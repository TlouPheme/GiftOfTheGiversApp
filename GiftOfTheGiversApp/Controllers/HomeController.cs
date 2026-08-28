using System.Diagnostics;
using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using GiftOfTheGiversApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var active = await _db.Incidents
            .Where(i => i.IsActive)
            .OrderByDescending(i => i.StartDate)
            .Take(4)
            .ToListAsync();

        var amounts = await _db.MonetaryDonations.Select(d => d.AmountZar).ToListAsync();
        var goodsCounts = await _db.GoodsDonations.Select(d => d.ItemCount).ToListAsync();

        var model = new HomeDashboardViewModel
        {
            MoneyTotal = amounts.Sum(),
            GoodsTotal = goodsCounts.Sum(),
            ActiveIncidentCount = await _db.Incidents.CountAsync(i => i.IsActive),
            ActiveIncidents = active
        };
        return View(model);
    }

    public async Task<IActionResult> Transparency()
    {
        var categories = await _db.GoodsCategories.OrderBy(c => c.Name).ToListAsync();
        var goods = await _db.GoodsDonations.ToListAsync();
        var amounts = await _db.MonetaryDonations.Select(d => d.AmountZar).ToListAsync();

        var model = new TransparencyViewModel
        {
            MoneyTotal = amounts.Sum(),
            GoodsTotal = goods.Sum(d => d.ItemCount),
            ActiveIncidents = await _db.Incidents.Where(i => i.IsActive).OrderByDescending(i => i.StartDate).ToListAsync(),
            GoodsByCategory = categories.Select(c => new CategoryTotal
            {
                Name = c.Name,
                Description = c.Description,
                ItemCount = goods.Where(g => g.CategoryId == c.Id).Sum(g => g.ItemCount)
            }).ToList()
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
