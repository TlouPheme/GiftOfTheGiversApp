using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using GiftOfTheGiversApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfTheGivers.Helpers;

namespace GiftOfTheGiversApp.Controllers;

public class DonationsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public DonationsController(ApplicationDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Create(string tab = "money")
    {
        return View(await BuildPage(tab));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMoney([Bind(Prefix = "Money")] MonetaryDonation money)
    {
        if (money.IsAnonymous)
        {
            money.DonorName = null;
        }
        else if (string.IsNullOrWhiteSpace(money.DonorName))
        {
            ModelState.AddModelError("Money.DonorName", "Enter a donor name, or choose to remain anonymous.");
        }

        if (!ModelState.IsValid)
        {
            var page = await BuildPage("money");
            page.Money = money;
            return View("Create", page);
        }

        _db.MonetaryDonations.Add(money);
        await _db.SaveChangesAsync();
        var certificateNumber = await RequestCertificateNumber(money);
        TempData["Status"] = money.IsAnonymous
            ? $"Anonymous monetary donation captured. Thank you. Tax certificate: {certificateNumber}."
            : $"Thank you, {money.DonorName}. Your monetary donation has been captured. Your tax certificate number is {certificateNumber}.";
        return RedirectToAction(nameof(Create), new { tab = "money" });
    }

    public IActionResult TaxCertificate()
    {
        return View(new TaxCertificateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TaxCertificate(TaxCertificateViewModel lookup)
    {
        if (!ModelState.IsValid)
        {
            return View(lookup);
        }

        // The amount acts as a light check, so nobody can pull up another donor's
        // certificate just by guessing numbers.
        MonetaryDonation? donation = null;
        if (TryParseCertificateNumber(lookup.CertificateNumber, out var donationId))
        {
            donation = await _db.MonetaryDonations.FirstOrDefaultAsync(d => d.Id == donationId);
        }

        if (donation is null || donation.AmountZar != lookup.AmountZar)
        {
            ModelState.AddModelError(string.Empty,
                "We could not find a monetary donation with that certificate number and amount. Check both and try again.");
            return View(lookup);
        }

        lookup.Donation = donation;
        lookup.IssuedCertificateNumber = await RequestCertificateNumber(donation);
        return View(lookup);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateGoods([Bind(Prefix = "Goods")] GoodsDonation goods)
    {
        if (goods.IsAnonymous)
        {
            goods.DonorName = null;
        }
        else if (string.IsNullOrWhiteSpace(goods.DonorName))
        {
            ModelState.AddModelError("Goods.DonorName", "Enter a donor name, or choose to remain anonymous.");
        }

        if (!ModelState.IsValid)
        {
            var page = await BuildPage("goods");
            page.Goods = goods;
            return View("Create", page);
        }

        _db.GoodsDonations.Add(goods);
        await _db.SaveChangesAsync();
        TempData["Status"] = "Goods donation captured. The public item total has been updated.";
        return RedirectToAction(nameof(Create), new { tab = "goods" });
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        ViewBag.Money = await _db.MonetaryDonations.OrderByDescending(d => d.DonationDate).ToListAsync();
        ViewBag.Goods = await _db.GoodsDonations.Include(d => d.Category).OrderByDescending(d => d.DonationDate).ToListAsync();
        return View();
    }

    // Reads the donation id back out of a certificate number such as GOTG-2026-000042.
    // The year is not checked, because the function stamps the year it was called in.
    private static bool TryParseCertificateNumber(string? certificateNumber, out int donationId)
    {
        donationId = 0;
        var match = System.Text.RegularExpressions.Regex.Match(
            certificateNumber?.Trim() ?? "",
            @"^GOTG-\d{4}-(\d{6,9})$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return match.Success
            && int.TryParse(match.Groups[1].Value, out donationId)
            && donationId > 0;
    }

    private async Task<string> RequestCertificateNumber(MonetaryDonation money)
    {
        var certificateNumber = DonationHelper.FormatCertificateNumber(money.Id, money.DonationDate);
        var http = _httpClientFactory.CreateClient("TaxCertificate");
        var url =
            "api/GenerateTaxCertificate" +
            $"?donationId={money.Id}" +
            $"&amount={money.AmountZar.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
            $"&donorName={Uri.EscapeDataString(money.DonorName ?? "")}" +
            $"&anonymous={money.IsAnonymous.ToString().ToLowerInvariant()}";
        try
        {
            var json = await http.GetStringAsync(url);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            certificateNumber = doc.RootElement.GetProperty("certificateNumber").GetString() ?? certificateNumber;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException
            or System.Text.Json.JsonException or KeyNotFoundException)
        {
            // The donation is already saved, so a function outage must not lose it.
            // Fall back to the locally formatted certificate number.
        }

        return certificateNumber;
    }

    private async Task<DonatePageViewModel> BuildPage(string tab)
    {
        return new DonatePageViewModel
        {
            ActiveTab = tab == "goods" ? "goods" : "money",
            Categories = await _db.GoodsCategories.OrderBy(c => c.Name).ToListAsync(),
            Money = new MonetaryDonation { DonationDate = DateTime.Today },
            Goods = new GoodsDonation { DonationDate = DateTime.Today }
        };
    }
}