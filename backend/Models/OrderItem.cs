using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        
        public string ProductImageUrl { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal TotalPrice { get; set; }
    }
} 