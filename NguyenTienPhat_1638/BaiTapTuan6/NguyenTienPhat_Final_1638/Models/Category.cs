using System.ComponentModel.DataAnnotations;

namespace NguyenTienPhat_Final_1638.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
        public string Name { get; set; } = null!;
        
        public List<Product>? Products { get; set; }
    }
}
