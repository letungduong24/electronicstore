using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Brand { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Power { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Material { get; set; }
    
    [StringLength(200)]
    public string Image { get; set; }

    public string ProductType { get; set; }

    // Đặc trưng cho từng loại sản phẩm
    public string? Scope { get; set; } // AirConditioner
    public int? ScreenSize { get; set; } // Television
    public int? Capacity { get; set; } // WashingMachine
} 