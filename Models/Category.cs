using System.ComponentModel.DataAnnotations;

namespace BookDB.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a category name")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
