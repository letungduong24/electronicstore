using UserManagementAPI.Models;
using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services.Factories
{
    public interface IProductFactory
    {
        ProductModel CreateProduct(string type);
        ProductModel CreateProductFromDto(ProductDTO productDto);
        bool ValidateProductType(string type);
        string[] GetSupportedProductTypes();
        Dictionary<string, string[]> GetRequiredPropertiesForType(string type);
        bool ValidateProductProperties(string type, Dictionary<string, object> properties);
    }
} 