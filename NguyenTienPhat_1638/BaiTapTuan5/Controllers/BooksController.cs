using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookDB.Data;
using BookDB.Models;

namespace BookDB.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;
        private const int PageSize = 5;

        public BooksController(BookDbContext context)
        {
            _context = context;
        }

        // GET: Books
        // GET: Books?categoryId=1&page=2
        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            // Ensure page is at least 1
            if (page < 1)
            {
                page = 1;
            }

            // Start with all books query
            IQueryable<Book> booksQuery = _context.Books.Include(b => b.Category);

            // Apply category filter if provided
            if (categoryId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            // Get total count for pagination calculation
            int totalBooks = await booksQuery.CountAsync();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling(totalBooks / (double)PageSize);

            // Ensure page doesn't exceed total pages
            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            // Apply pagination
            var books = await booksQuery
                .OrderBy(b => b.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Get category name if filtering
            string? categoryName = null;
            if (categoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(categoryId.Value);
                categoryName = category?.Name;
            }

            // Create view model
            var viewModel = new BookListViewModel
            {
                Books = books,
                CurrentPage = page,
                TotalPages = totalPages,
                SelectedCategoryId = categoryId,
                SelectedCategoryName = categoryName
            };

            return View(viewModel);
        }
    }
}
