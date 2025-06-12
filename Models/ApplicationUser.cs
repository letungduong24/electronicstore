using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int Role { get; set; }
        public string? PhoneNumber { get; set; } 
    }
    public enum UserRole
    {
        Admin = 0,
        User = 1
    }

} 