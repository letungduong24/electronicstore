using ElectronicStore.DTOs;
using ElectronicStore.Models;

namespace ElectronicStore.Mappers.Strategies;

public class TelevisionMappingStrategy : IProductMappingStrategy
{
    public bool CanMap(ProductDto dto) => dto.ProductType?.ToLower() == "television";
    
    public bool CanMapModel(ProductModel model) => model is Television;

    public void MapSpecificProperties(ProductModel product, ProductDto dto)
    {
        if (product is Television television)
        {
            television.ScreenSize = dto.ScreenSize ?? 0;
        }
    }

    public ProductDto CreateDto(ProductModel model)
    {
        if (model is Television tv)
        {
            return new TelevisionDto { ScreenSize = tv.ScreenSize };
        }
        return new ProductDto();
    }
} 