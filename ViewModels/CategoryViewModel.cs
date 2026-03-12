using BookDB.Models;

namespace BookDB.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BookCount { get; set; }
        public List<Book> SampleBooks { get; set; } = new List<Book>();
    }
}
