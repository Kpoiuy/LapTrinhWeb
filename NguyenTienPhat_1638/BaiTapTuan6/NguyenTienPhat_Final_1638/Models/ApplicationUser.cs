using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations; 

namespace NguyenTienPhat_Final_1638.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        
        public string FullName { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? Age { get; set; }

        // Navigation property
        public List<Order>? Orders { get; set; }
    }
}