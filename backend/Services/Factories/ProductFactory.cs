using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Models;
using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services.Factories
{
    public class ProductFactory : IProductFactory
    {
        private readonly Dictionary<string, string[]> _requiredProperties = new()
        {
            { "tv", new[] { "ScreenSize" } },
            { "airconditioner", new[] { "Scope" } },
            { "washingmachine", new[] { "Capacity" } }
        };

        private readonly string[] _supportedTypes = { "tv", "airconditioner", "washingmachine" };

        private readonly Dictionary<string, string> _typeDisplayNames = new()
        {
            { "tv", "Tivi" },
            { "airconditioner", "Điều hòa" },
            { "washingmachine", "Máy giặt" }
        };

        public ProductModel CreateProduct(string type)
        {
            if (!ValidateProductType(type))
                throw new ArgumentException($"Unsupported product type: {type}");

            return type.ToLower() switch
            {
                "tv" => new Television(),
                "airconditioner" => new AirConditioner(),
                "washingmachine" => new WashingMachine(),
                _ => throw new ArgumentException("Unknown product type")
            };
        }

        public ProductModel CreateProductFromDto(ProductDTO productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            if (!ValidateProductType(productDto.Type))
                throw new ArgumentException($"Unsupported product type: {productDto.Type}");

            var product = CreateProduct(productDto.Type);
            
            // Set common properties
            product.ID = productDto.ID;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.Brand = productDto.Brand;
            product.Model = productDto.Model;
            product.ImageUrl = productDto.ImageUrl;
            product.Type = productDto.Type;

            // Set specific properties if provided
            if (productDto.Properties != null && productDto.Properties.Any())
            {
                product.SetSpecificProperties(productDto.Properties);
            }

            return product;
        }

        public bool ValidateProductType(string type)
        {
            return !string.IsNullOrEmpty(type) && _supportedTypes.Contains(type.ToLower());
        }

        public string[] GetSupportedProductTypes()
        {
            return _supportedTypes;
        }

        public Dictionary<string, string> GetTypeDisplayNames()
        {
            return _typeDisplayNames;
        }

        public Dictionary<string, string[]> GetRequiredPropertiesForType(string type)
        {
            if (!ValidateProductType(type))
                throw new ArgumentException($"Unsupported product type: {type}");

            return new Dictionary<string, string[]> { { type.ToLower(), _requiredProperties[type.ToLower()] } };
        }

        public bool ValidateProductProperties(string type, Dictionary<string, object> properties)
        {
            if (!ValidateProductType(type))
                return false;

            var requiredProps = _requiredProperties[type.ToLower()];
            return requiredProps.All(prop => properties.ContainsKey(prop));
        }
    }
} 