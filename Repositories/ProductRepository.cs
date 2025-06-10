using ElectronicStore.Models;
using ElectronicStore.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectronicStore.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ProductModel>> GetAllAsync() => await _context.Products.ToListAsync();
    public async Task<ProductModel> GetOneAsync(int id) => await _context.Products.FindAsync(id);
    public async Task CreateAsync(ProductModel product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(ProductModel product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
} 