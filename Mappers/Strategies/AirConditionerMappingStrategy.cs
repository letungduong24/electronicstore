using ElectronicStore.DTOs;
using ElectronicStore.Models;

namespace ElectronicStore.Mappers.Strategies;

public class AirConditionerMappingStrategy : IProductMappingStrategy
{
    public bool CanMap(ProductDto dto) => dto.ProductType?.ToLower() == "airconditioner";
    
    public bool CanMapModel(ProductModel model) => model is AirConditioner;

    public void MapSpecificProperties(ProductModel product, ProductDto dto)
    {
        if (product is AirConditioner airConditioner)
        {
            airConditioner.Scope = dto.Scope;
        }
    }

    public ProductDto CreateDto(ProductModel model)
    {
        if (model is AirConditioner ac)
        {
            return new AirConditionerDto { Scope = ac.Scope };
        }
        return new ProductDto();
    }
} 