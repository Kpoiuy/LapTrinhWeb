using System.ComponentModel.DataAnnotations;

namespace BookDB.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a book title")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an author name")]
        [StringLength(100, ErrorMessage = "Author cannot exceed 100 characters")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a year")]
        [Range(1000, 3000, ErrorMessage = "Year must be between 1000 and 3000")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please enter a publisher")]
        [StringLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string? ImageUrl { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater")]
        public decimal? Price { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }
    }
}
