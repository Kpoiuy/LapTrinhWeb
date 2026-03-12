using BookDB.Data;
using BookDB.Models;
using BookDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookDB.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;
        private readonly ILogger<BooksController> _logger;
        private const int PageSize = 5;

        public BooksController(BookDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            try
            {
                var query = _context.Books.Include(b => b.Category).AsQueryable();

                string? categoryName = null;
                if (categoryId.HasValue)
                {
                    query = query.Where(b => b.CategoryId == categoryId.Value);
                    var category = await _context.Categories.FindAsync(categoryId.Value);
                    if (category == null)
                    {
                        _logger.LogWarning("Category with ID {CategoryId} not found", categoryId.Value);
                        return NotFound("Category not found");
                    }
                    categoryName = category.Name;
                }

                var totalBooks = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalBooks / (double)PageSize);

                if (page < 1) page = 1;
                if (page > totalPages && totalPages > 0) page = totalPages;

                var books = await query
                    .OrderBy(b => b.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                var viewModel = new BookListViewModel
                {
                    Books = books,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    SelectedCategoryId = categoryId,
                    SelectedCategoryName = categoryName
                };

                _logger.LogInformation("Loaded books page {Page} with {Count} books", page, books.Count);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books");
                return StatusCode(500, "An error occurred while loading books");
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found", id);
                    return NotFound("Book not found");
                }

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book details for ID {BookId}", id);
                return StatusCode(500, "An error occurred while loading book details");
            }
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Year,Publisher,CategoryId,ImageUrl,Price,Rating")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created book with ID {BookId}", book.Id);
                    return RedirectToAction(nameof(Details), new { id = book.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating book");
                    ModelState.AddModelError("", "An error occurred while creating the book");
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for editing", id);
                    return NotFound("Book not found");
                }

                ViewBag.Categories = _context.Categories.ToList();
                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book for editing, ID {BookId}", id);
                return StatusCode(500, "An error occurred while loading the book");
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Year,Publisher,CategoryId,ImageUrl,Price,Rating")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Updated book with ID {BookId}", book.Id);
                    return RedirectToAction(nameof(Details), new { id = book.Id });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!BookExists(book.Id))
                    {
                        _logger.LogWarning("Book with ID {BookId} not found during update", book.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Concurrency error updating book with ID {BookId}", book.Id);
                        ModelState.AddModelError("", "The book was modified by another user. Please reload and try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating book with ID {BookId}", book.Id);
                    ModelState.AddModelError("", "An error occurred while updating the book");
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
                    return NotFound("Book not found");
                }

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book for deletion, ID {BookId}", id);
                return StatusCode(500, "An error occurred while loading the book");
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted book with ID {BookId}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                return StatusCode(500, "An error occurred while deleting the book");
            }
        }

        // POST: Books/AddToCart/5
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Placeholder implementation for cart functionality
            TempData["Message"] = $"Book {id} added to cart";
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Order/5
        public IActionResult Order(int id)
        {
            // Placeholder implementation for order functionality
            TempData["Message"] = $"Order initiated for book {id}";
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
