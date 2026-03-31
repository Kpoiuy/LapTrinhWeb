using BookAPI.Data;
using BookAPI.Models;
using BookAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

public class CategoriesMvcController(BookDbContext dbContext) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword)
    {
        var query = dbContext.Categories.Include(x => x.Books).AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var key = keyword.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(key));
        }

        ViewBag.Keyword = keyword;
        var categories = await query.OrderBy(x => x.Id).ToListAsync();
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var category = await dbContext.Categories
            .Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Create() => View(new CategoryFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var exists = await dbContext.Categories.AnyAsync(x => x.Name.ToLower() == model.Name.Trim().ToLower());
        if (exists)
        {
            ModelState.AddModelError("Name", "Category đã tồn tại.");
            return View(model);
        }

        dbContext.Categories.Add(new Category { Name = model.Name.Trim() });
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound();
        }
        return View(new CategoryFormViewModel { Id = category.Id, Name = category.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = await dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        var exists = await dbContext.Categories.AnyAsync(x => x.Id != id && x.Name.ToLower() == model.Name.Trim().ToLower());
        if (exists)
        {
            ModelState.AddModelError("Name", "Category đã tồn tại.");
            return View(model);
        }

        category.Name = model.Name.Trim();
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await dbContext.Categories.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await dbContext.Categories.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return NotFound();
        }
        if (category.Books.Any())
        {
            TempData["Error"] = "Không thể xóa category đang có sách.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
