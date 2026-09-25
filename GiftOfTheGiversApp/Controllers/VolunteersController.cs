using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

public class VolunteersController : Controller
{
    private readonly ApplicationDbContext _db;

    public VolunteersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Create()
    {
        return View(new VolunteerApplication());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VolunteerApplication volunteer)
    {
        if (!ModelState.IsValid)
        {
            return View(volunteer);
        }

        volunteer.SubmittedAt = DateTime.UtcNow;
        _db.VolunteerApplications.Add(volunteer);
        await _db.SaveChangesAsync();
        TempData["Status"] = "Volunteer application captured. Operations staff can see your skills and availability on the desk.";
        return RedirectToAction(nameof(Create));
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var list = await _db.VolunteerApplications.OrderByDescending(v => v.SubmittedAt).ToListAsync();
        return View(list);
    }
}
