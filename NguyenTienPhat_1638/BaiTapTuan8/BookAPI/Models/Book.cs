using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(1000, 3000)]
    public int Year { get; set; }

    [Required]
    [StringLength(100)]
    public string Publisher { get; set; } = string.Empty;

    [Range(0, 1000000000)]
    public decimal Price { get; set; }

    public string? ImagePath { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
