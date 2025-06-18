using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class UpdateBalanceDto
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative")]
        public decimal NewBalance { get; set; }
        
        public string? Reason { get; set; }
    }
} 