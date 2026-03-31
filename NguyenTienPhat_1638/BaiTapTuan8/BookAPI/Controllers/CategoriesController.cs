using BookAPI.Data;
using BookAPI.Dtos;
using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(BookDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Search([FromQuery] string? keyword)
    {
        var query = dbContext.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var key = keyword.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(key));
        }

        var categories = await query
            .OrderBy(x => x.Id)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound(new { message = "Category not found." });
        }

        return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryUpsertRequest request)
    {
        var name = request.Name.Trim();
        var exists = await dbContext.Categories.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        if (exists)
        {
            return BadRequest(new { message = "Category already exists." });
        }

        var category = new Category { Name = name };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var result = new CategoryDto { Id = category.Id, Name = category.Name };
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] CategoryUpsertRequest request)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return NotFound(new { message = "Category not found." });
        }

        var name = request.Name.Trim();
        var exists = await dbContext.Categories.AnyAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());
        if (exists)
        {
            return BadRequest(new { message = "Category already exists." });
        }

        category.Name = name;
        await dbContext.SaveChangesAsync();
        return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await dbContext.Categories.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return NotFound(new { message = "Category not found." });
        }

        if (category.Books.Any())
        {
            return BadRequest(new { message = "Cannot delete category that still has books." });
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
