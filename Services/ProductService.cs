using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UserManagementAPI.Models;
using UserManagementAPI.DTOs;
using UserManagementAPI.Repositories;
using UserManagementAPI.Services.Factories;
using UserManagementAPI.Mapper;

namespace UserManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductFactory _productFactory;
        private readonly ProductMapper _productMapper;

        public ProductService(
            IProductRepository productRepository,
            IProductFactory productFactory,
            ProductMapper productMapper)
        {
            _productRepository = productRepository;
            _productFactory = productFactory;
            _productMapper = productMapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(p => _productMapper.ToDTO(p));
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product != null ? _productMapper.ToDTO(product) : null;
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
        {
            var product = _productMapper.ToModel(productDto);
            await _productRepository.AddProductAsync(product);
            return _productMapper.ToDTO(product);
        }

        public async Task UpdateProductAsync(int id, ProductDTO productDto)
        {
            if (id != productDto.ID)
            {
                throw new ArgumentException("Product ID mismatch");
            }

            if (!await ProductExistsAsync(id))
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            var product = _productMapper.ToModel(productDto);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            if (!await ProductExistsAsync(id))
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.ProductExistsAsync(id);
        }

        public void SetProperties(ProductModel product, Dictionary<string, object> values)
        {
            product.SetSpecificProperties(values);
        }
    }
}