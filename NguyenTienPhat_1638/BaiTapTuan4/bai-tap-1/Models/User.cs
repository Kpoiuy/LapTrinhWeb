using System.ComponentModel.DataAnnotations;

namespace BaiTap1.Models;

public class User
{
    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Bắt buộc nhập tên đăng nhập")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ Email")]
    [Required(ErrorMessage = "Bắt buộc nhập Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Bắt buộc nhập mật khẩu")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Xác thực mật khẩu")]
    [Required(ErrorMessage = "Bắt buộc nhập xác thực mật khẩu")]
    [Compare("Password", ErrorMessage = "Mật khẩu và xác thực mật khẩu không trùng khớp")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
