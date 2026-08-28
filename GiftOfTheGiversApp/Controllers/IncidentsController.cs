using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

public class IncidentsController : Controller
{
    private readonly ApplicationDbContext _db;

    public IncidentsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var incidents = await _db.Incidents
            .OrderByDescending(i => i.IsActive)
            .ThenByDescending(i => i.StartDate)
            .ToListAsync();
        return View(incidents);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View(new Incident { StartDate = DateTime.Today, IsActive = true });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Incident incident)
    {
        if (!ModelState.IsValid)
        {
            return View(incident);
        }

        incident.IsActive = incident.EndDate is null;
        _db.Incidents.Add(incident);
        await _db.SaveChangesAsync();
        TempData["Status"] = "Incident captured. It now appears on the public incidents list.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident is null)
        {
            return NotFound();
        }

        incident.IsActive = false;
        incident.EndDate ??= DateTime.Today;
        await _db.SaveChangesAsync();
        TempData["Status"] = $"{incident.Title} was closed.";
        return RedirectToAction(nameof(Index));
    }
}
