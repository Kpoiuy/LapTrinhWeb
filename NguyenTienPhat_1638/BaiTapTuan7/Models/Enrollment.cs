using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BaiTapTuan7.Models;

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int CourseId { get; set; }

    public DateTime EnrollDate { get; set; } = DateTime.UtcNow;

    public IdentityUser? User { get; set; }
    public Course? Course { get; set; }
}
