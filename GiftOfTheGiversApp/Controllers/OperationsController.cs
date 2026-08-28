using GiftOfTheGiversApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

[Authorize]
public class OperationsController : Controller
{
    private readonly ApplicationDbContext _db;

    public OperationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var amounts = await _db.MonetaryDonations.Select(d => d.AmountZar).ToListAsync();
        ViewBag.MoneyTotal = amounts.Sum();
        ViewBag.GoodsTotal = await _db.GoodsDonations.SumAsync(d => d.ItemCount);
        ViewBag.ActiveIncidents = await _db.Incidents.CountAsync(i => i.IsActive);
        ViewBag.Volunteers = await _db.VolunteerApplications.CountAsync();
        return View();
    }
}
