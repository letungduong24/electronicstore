using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string ShippingAddress { get; set; }
        
        public string? Notes { get; set; }
    }
} 