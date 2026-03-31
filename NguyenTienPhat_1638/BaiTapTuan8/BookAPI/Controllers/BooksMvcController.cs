using BookAPI.Data;
using BookAPI.Models;
using BookAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

public class BooksMvcController(BookDbContext dbContext, IWebHostEnvironment environment) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? keyword, int? categoryId)
    {
        var query = dbContext.Books.Include(x => x.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var key = keyword.Trim().ToLower();
            query = query.Where(x =>
                x.Title.ToLower().Contains(key) ||
                x.Author.ToLower().Contains(key) ||
                x.Publisher.ToLower().Contains(key));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        ViewBag.Keyword = keyword;
        ViewBag.CategoryId = categoryId;
        ViewBag.Categories = await dbContext.Categories
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToListAsync();

        var books = await query.OrderByDescending(x => x.Id).ToListAsync();
        return View(books);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var book = await dbContext.Books.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (book is null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = await BuildFormModelAsync(new BookFormViewModel());
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildFormModelAsync(model));
        }

        var imagePath = await SaveImageAsync(model.ImageFile);
        if (!ModelState.IsValid)
        {
            return View(await BuildFormModelAsync(model));
        }
        var book = new Book
        {
            Title = model.Title.Trim(),
            Author = model.Author.Trim(),
            Year = model.Year,
            Publisher = model.Publisher.Trim(),
            Price = model.Price,
            CategoryId = model.CategoryId,
            ImagePath = imagePath ?? "/Content/ImageBooks/seed-book-1.jpg"
        };

        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        var vm = new BookFormViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Publisher = book.Publisher,
            Price = book.Price,
            CategoryId = book.CategoryId,
            CurrentImagePath = book.ImagePath
        };
        return View(await BuildFormModelAsync(vm));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.CurrentImagePath = book.ImagePath;
            return View(await BuildFormModelAsync(model));
        }

        var newImage = await SaveImageAsync(model.ImageFile);
        if (!ModelState.IsValid)
        {
            model.CurrentImagePath = book.ImagePath;
            return View(await BuildFormModelAsync(model));
        }

        book.Title = model.Title.Trim();
        book.Author = model.Author.Trim();
        book.Year = model.Year;
        book.Publisher = model.Publisher.Trim();
        book.Price = model.Price;
        book.CategoryId = model.CategoryId;

        if (!string.IsNullOrWhiteSpace(newImage))
        {
            DeleteUploadedImageIfNeeded(book.ImagePath);
            book.ImagePath = newImage;
        }

        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await dbContext.Books.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (book is null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        DeleteUploadedImageIfNeeded(book.ImagePath);
        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<BookFormViewModel> BuildFormModelAsync(BookFormViewModel model)
    {
        model.Categories = await dbContext.Categories
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToListAsync();
        return model;
    }

    private async Task<string?> SaveImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension is not ".jpg" and not ".jpeg" and not ".png")
        {
            ModelState.AddModelError("ImageFile", "Chỉ chấp nhận file .jpg/.jpeg/.png");
            return null;
        }

        var fileName = $"upload-{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(environment.ContentRootPath, "Content", "ImageBooks", fileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return $"/Content/ImageBooks/{fileName}";
    }

    private void DeleteUploadedImageIfNeeded(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }
        if (!imagePath.Contains("/Content/ImageBooks/upload-", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(environment.ContentRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
}
