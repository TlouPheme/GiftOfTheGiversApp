using GiftOfTheGiversApp.Data;
using GiftOfTheGiversApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGiversApp.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _db.GoodsCategories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.NewCategory = new GoodsCategory();
        return View(list);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GoodsCategory category)
    {
        if (await _db.GoodsCategories.AnyAsync(c => c.Name == category.Name))
        {
            ModelState.AddModelError("Name", "That category already exists.");
        }

        if (!ModelState.IsValid)
        {
            var list = await _db.GoodsCategories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.NewCategory = category;
            return View("Index", list);
        }

        category.IsPredefined = false;
        _db.GoodsCategories.Add(category);
        await _db.SaveChangesAsync();
        TempData["Status"] = "Category added. Donors can select it on the goods form.";
        return RedirectToAction(nameof(Index));
    }
}
