using ElectronicStore.DTOs;
using ElectronicStore.Models;

namespace ElectronicStore.Mappers;

public interface IProductMappingStrategy
{
    bool CanMap(ProductDto dto);
    bool CanMapModel(ProductModel model);
    void MapSpecificProperties(ProductModel product, ProductDto dto);
    ProductDto CreateDto(ProductModel model);
} 