using System.ComponentModel.DataAnnotations;

namespace BaiTap2.Models;

public class Todo
{
    [Display(Name = "Mã công việc")]
    [Required(ErrorMessage = "Mã công việc không được để trống")]
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Tên công việc")]
    [Required(ErrorMessage = "Tên công việc không được để trống")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Trạng thái hoàn thành")]
    public bool Completed { get; set; }
}
