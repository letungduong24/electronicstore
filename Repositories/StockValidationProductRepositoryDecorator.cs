namespace ElectronicStore.Repositories;

using ElectronicStore.Models;

public class StockValidationProductRepositoryDecorator : IProductRepository
{
    private readonly IProductRepository _inner;

    public StockValidationProductRepositoryDecorator(IProductRepository inner)
    {
        _inner = inner;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync() => await _inner.GetAllAsync();

    public async Task<ProductModel> GetOneAsync(int id) => await _inner.GetOneAsync(id);

    public async Task CreateAsync(ProductModel product)
    {
        if (product.Stock < 0)
            throw new InvalidOperationException("Stock must be >= 0");
        await _inner.CreateAsync(product);
    }

    public async Task UpdateAsync(ProductModel product)
    {
        if (product.Stock < 0)
            throw new InvalidOperationException("Stock must be >= 0");
        await _inner.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _inner.GetOneAsync(id);
        if (product != null && product.Stock > 0)
            throw new InvalidOperationException("Cannot delete product with stock > 0");
        await _inner.DeleteAsync(id);
    }
} 