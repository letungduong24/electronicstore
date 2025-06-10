using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.DTOs;

public class TelevisionDto : ProductDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ScreenSize { get; set; }
} 