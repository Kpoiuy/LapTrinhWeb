using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookAPI.ViewModels;

public class BookFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(1000, 3000)]
    public int Year { get; set; } = DateTime.Now.Year;

    [Required]
    [StringLength(100)]
    public string Publisher { get; set; } = string.Empty;

    [Range(0, 1000000000)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public string? CurrentImagePath { get; set; }
    public IFormFile? ImageFile { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();
}
