using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.DTOs;

public class WashingMachineDto : ProductDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
} 