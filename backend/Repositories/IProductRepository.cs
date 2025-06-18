using UserManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagementAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task<ProductModel> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductModel product);
        Task UpdateProductAsync(ProductModel product);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
} 