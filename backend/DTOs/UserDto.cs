using System.Collections.Generic;

namespace UserManagementAPI.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<string>? Roles { get; set; }
        public decimal Balance { get; set; }
        public int CartItemCount { get; set; }
    }
} 