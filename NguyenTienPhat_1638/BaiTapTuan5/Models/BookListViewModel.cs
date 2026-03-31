namespace BookDB.Models
{
    public class BookListViewModel
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string? SelectedCategoryName { get; set; }
    }
}
