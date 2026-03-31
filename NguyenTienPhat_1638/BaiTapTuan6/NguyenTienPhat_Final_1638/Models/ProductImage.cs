namespace NguyenTienPhat_Final_1638.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
