using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
        Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
        string[] GetSupportedProductTypes();
        Dictionary<string, string> GetTypeDisplayNames();
        Dictionary<string, string[]> GetRequiredPropertiesForType(string type);
        Task<IEnumerable<ProductDTO>> GetLatestProductsAsync(int count);
    }
}