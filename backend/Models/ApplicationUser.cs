using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        
        [Required]
        public decimal Balance { get; set; } = 0;
        
        // Navigation property
        public Cart Cart { get; set; }
    }
} 