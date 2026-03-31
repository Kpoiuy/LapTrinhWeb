using System.ComponentModel.DataAnnotations;

namespace BaiTapTuan7.Models;

public class Course
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên học phần là bắt buộc.")]
    [StringLength(200)]
    [Display(Name = "Tên học phần")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Đường dẫn ảnh tối đa 2000 ký tự.")]
    [Display(Name = "Đường dẫn ảnh")]
    public string? Image { get; set; }

    [Display(Name = "Số tín chỉ")]
    [Range(1, 10, ErrorMessage = "Số tín chỉ phải nằm trong khoảng từ 1 đến 10.")]
    public int Credits { get; set; }

    [Required(ErrorMessage = "Tên giảng viên là bắt buộc.")]
    [StringLength(120)]
    [Display(Name = "Giảng viên")]
    public string Lecturer { get; set; } = string.Empty;

    [Display(Name = "Danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
