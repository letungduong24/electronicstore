using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.DTOs;

public class AirConditionerDto : ProductDto
{
    [Required]
    [StringLength(50)]
    public string Scope { get; set; }
} 