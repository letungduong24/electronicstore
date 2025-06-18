using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class AssignRoleDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
} 