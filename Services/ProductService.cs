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
            // Validate product type
            if (!_productFactory.ValidateProductType(productDto.Type))
            {
                throw new ArgumentException($"Invalid product type: {productDto.Type}. Supported types: {string.Join(", ", _productFactory.GetSupportedProductTypes())}");
            }

            // Validate required properties
            if (productDto.Properties != null && !_productFactory.ValidateProductProperties(productDto.Type, productDto.Properties))
            {
                var requiredProps = _productFactory.GetRequiredPropertiesForType(productDto.Type)[productDto.Type.ToLower()];
                throw new ArgumentException($"Missing required properties for {productDto.Type}: {string.Join(", ", requiredProps)}");
            }

            // Create product using factory
            var product = _productFactory.CreateProductFromDto(productDto);
            
            // Validate product
            if (!product.ValidateProduct())
            {
                throw new ArgumentException("Product validation failed");
            }

            await _productRepository.AddProductAsync(product);
            return _productMapper.ToDTO(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, ProductDTO productDto)
        {
            if (id != productDto.ID)
            {
                throw new ArgumentException("Product ID mismatch");
            }

            if (!await ProductExistsAsync(id))
            {
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            // Validate product type
            if (!_productFactory.ValidateProductType(productDto.Type))
            {
                throw new ArgumentException($"Invalid product type: {productDto.Type}");
            }

            var product = _productFactory.CreateProductFromDto(productDto);
            
            if (!product.ValidateProduct())
            {
                throw new ArgumentException("Product validation failed");
            }

            await _productRepository.UpdateProductAsync(product);
            
            // Return the updated product
            return _productMapper.ToDTO(product);
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

        public string[] GetSupportedProductTypes()
        {
            return _productFactory.GetSupportedProductTypes();
        }

        public Dictionary<string, string[]> GetRequiredPropertiesForType(string type)
        {
            return _productFactory.GetRequiredPropertiesForType(type);
        }

        public void SetProperties(ProductModel product, Dictionary<string, object> values)
        {
            product.SetSpecificProperties(values);
        }
    }
}