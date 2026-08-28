using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using GiftOfTheGiversApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

public class DonationsController : Controller
{
    private readonly ApplicationDbContext _db;

    public DonationsController(ApplicationDbContext db)
    {
        _db = db;
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
        TempData["Status"] = money.IsAnonymous
            ? "Anonymous monetary donation captured. Thank you."
            : $"Thank you, {money.DonorName}. Your monetary donation has been captured.";
        return RedirectToAction(nameof(Create), new { tab = "money" });
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
