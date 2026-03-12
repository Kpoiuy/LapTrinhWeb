using BookDB.Data;
using BookDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookDB.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly BookDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(BookDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Books)
                    .ToListAsync();

                var viewModels = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BookCount = c.Books.Count,
                    SampleBooks = c.Books.Take(3).ToList()
                }).ToList();

                _logger.LogInformation("Loaded {Count} categories", viewModels.Count);
                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                return StatusCode(500, "An error occurred while loading categories");
            }
        }

        // GET: Categories/FilterByCategory/5
        public IActionResult FilterByCategory(int categoryId)
        {
            return RedirectToAction("Index", "Books", new { categoryId });
        }
    }
}
