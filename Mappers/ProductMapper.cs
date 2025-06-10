using ElectronicStore.DTOs;
using ElectronicStore.Models;
using ElectronicStore.Factories;

namespace ElectronicStore.Mappers;

public class ProductMapper
{
    private readonly IProductFactory _productFactory;
    private readonly IEnumerable<IProductMappingStrategy> _mappingStrategies;

    public ProductMapper(
        IProductFactory productFactory,
        IEnumerable<IProductMappingStrategy> mappingStrategies)
    {
        _productFactory = productFactory;
        _mappingStrategies = mappingStrategies;
    }

    public ProductModel MapDtoToModel(ProductDto dto)
    {
        var product = _productFactory.CreateProduct(GetProductType(dto));

        // Map common properties
        product.ID = dto.Id;
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.Brand = dto.Brand;
        product.Description = dto.Description;
        product.Power = dto.Power;
        product.Material = dto.Material;
        product.Image = dto.Image;

        // Map specific properties using strategy
        var strategy = _mappingStrategies.FirstOrDefault(s => s.CanMap(dto));
        strategy?.MapSpecificProperties(product, dto);

        return product;
    }

    public ProductDto MapModelToDto(ProductModel model)
    {
        // Find appropriate strategy based on model type
        var strategy = _mappingStrategies.FirstOrDefault(s => s.CanMapModel(model));
        var dto = strategy?.CreateDto(model) ?? new ProductDto();

        // Map common properties
        dto.Id = model.ID;
        dto.Name = model.Name;
        dto.Price = model.Price;
        dto.Stock = model.Stock;
        dto.Brand = model.Brand;
        dto.Description = model.Description;
        dto.Power = model.Power;
        dto.Material = model.Material;
        dto.Image = model.Image;

        return dto;
    }

    public void MapDtoToExistingModel(ProductDto dto, ProductModel model)
    {
        // Map thuộc tính chung
        model.Name = dto.Name;
        model.Price = dto.Price;
        model.Stock = dto.Stock;
        model.Brand = dto.Brand;
        model.Description = dto.Description;
        model.Power = dto.Power;
        model.Material = dto.Material;
        model.Image = dto.Image;

        // Map thuộc tính đặc trưng qua strategy
        var strategy = _mappingStrategies.FirstOrDefault(s => s.CanMap(dto));
        strategy?.MapSpecificProperties(model, dto);
    }
    
    private static FactoryProductType GetProductType(ProductDto dto)
    {
        return dto.ProductType?.ToLower() switch
        {
            "airconditioner" => FactoryProductType.AirConditioner,
            "television" => FactoryProductType.Television,
            "washingmachine" => FactoryProductType.WashingMachine,
            _ => throw new ArgumentException("Invalid product type")
        };
    }
} 