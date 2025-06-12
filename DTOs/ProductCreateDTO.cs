using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public int Power { get; set; }
        public string? Material { get; set; }
        public string? Image { get; set; }
        [Required]
        public string Type { get; set; }
        public string? Scope { get; set; }
        public string? Capacity { get; set; }
        public string? ScreenSize { get; set; }
    }
} 