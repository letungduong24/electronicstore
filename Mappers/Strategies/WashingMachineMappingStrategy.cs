using ElectronicStore.DTOs;
using ElectronicStore.Models;

namespace ElectronicStore.Mappers.Strategies;

public class WashingMachineMappingStrategy : IProductMappingStrategy
{
    public bool CanMap(ProductDto dto) => dto.ProductType?.ToLower() == "washingmachine";
    
    public bool CanMapModel(ProductModel model) => model is WashingMachine;

    public void MapSpecificProperties(ProductModel product, ProductDto dto)
    {
        if (product is WashingMachine washingMachine)
        {
            washingMachine.Capacity = dto.Capacity ?? 0;
        }
    }

    public ProductDto CreateDto(ProductModel model)
    {
        if (model is WashingMachine wm)
        {
            return new WashingMachineDto { Capacity = wm.Capacity };
        }
        return new ProductDto();
    }
} 