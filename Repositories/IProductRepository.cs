namespace ElectronicStore.Repositories;

using ElectronicStore.Models;
public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> GetAllAsync();
    Task<ProductModel> GetOneAsync(int id);
    Task CreateAsync(ProductModel product);
    Task UpdateAsync(ProductModel product);
    Task DeleteAsync(int id);
} 