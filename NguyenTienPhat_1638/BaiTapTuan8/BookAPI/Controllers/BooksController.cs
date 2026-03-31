using BookAPI.Data;
using BookAPI.Dtos;
using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(BookDbContext dbContext, IWebHostEnvironment environment) : ControllerBase
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png"
    };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> Search([FromQuery] string? keyword, [FromQuery] int? categoryId)
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

        var books = await query.OrderBy(x => x.Id).ToListAsync();
        return Ok(books.Select(MapToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await dbContext.Books.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        return Ok(MapToDto(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromForm] BookUpsertRequest request)
    {
        if (!await dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId))
        {
            return BadRequest(new { message = "CategoryId does not exist." });
        }

        var imageResult = await ValidateAndSaveImageAsync(request.ImageFile);
        if (!imageResult.Success)
        {
            return BadRequest(new { message = imageResult.Error });
        }

        var book = new Book
        {
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Year = request.Year,
            Publisher = request.Publisher.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId,
            ImagePath = imageResult.ImagePath ?? "/Content/ImageBooks/seed-book-1.jpg"
        };

        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync();

        await dbContext.Entry(book).Reference(x => x.Category).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, MapToDto(book));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookDto>> Update(int id, [FromForm] BookUpsertRequest request)
    {
        var book = await dbContext.Books.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        if (!await dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId))
        {
            return BadRequest(new { message = "CategoryId does not exist." });
        }

        string? newImagePath = null;
        if (request.ImageFile is not null)
        {
            var imageResult = await ValidateAndSaveImageAsync(request.ImageFile);
            if (!imageResult.Success)
            {
                return BadRequest(new { message = imageResult.Error });
            }
            newImagePath = imageResult.ImagePath;
        }

        book.Title = request.Title.Trim();
        book.Author = request.Author.Trim();
        book.Year = request.Year;
        book.Publisher = request.Publisher.Trim();
        book.Price = request.Price;
        book.CategoryId = request.CategoryId;

        if (!string.IsNullOrWhiteSpace(newImagePath))
        {
            DeleteImageIfExists(book.ImagePath);
            book.ImagePath = newImagePath;
        }

        await dbContext.SaveChangesAsync();
        await dbContext.Entry(book).Reference(x => x.Category).LoadAsync();
        return Ok(MapToDto(book));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        DeleteImageIfExists(book.ImagePath);
        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    private BookDto MapToDto(Book book)
    {
        string? imageUrl = null;
        
        if (!string.IsNullOrWhiteSpace(book.ImagePath))
        {
            // Nếu ImagePath là URL đầy đủ (http/https), dùng trực tiếp
            if (book.ImagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                book.ImagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                imageUrl = book.ImagePath;
            }
            // Nếu là đường dẫn local, thêm host
            else
            {
                imageUrl = $"{Request.Scheme}://{Request.Host}{book.ImagePath}";
            }
        }

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Publisher = book.Publisher,
            Price = book.Price,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? string.Empty,
            ImagePath = book.ImagePath,
            ImageUrl = imageUrl
        };
    }

    private async Task<(bool Success, string? ImagePath, string? Error)> ValidateAndSaveImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return (true, null, null);
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return (false, null, "Image size must be <= 5MB.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            return (false, null, "Only .jpg/.jpeg/.png images are allowed.");
        }

        var fileName = $"upload-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var physicalPath = Path.Combine(environment.ContentRootPath, "Content", "ImageBooks", fileName);

        await using var stream = new FileStream(physicalPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return (true, $"/Content/ImageBooks/{fileName}", null);
    }

    private void DeleteImageIfExists(string? imagePath)
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
