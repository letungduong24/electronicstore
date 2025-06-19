using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Models;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services.Factories;

namespace UserManagementAPI.Services.Factories
{
    public class ProductFactory : IProductFactory
    {
        private readonly List<ProductTypeDefinition> _productTypes = new()
        {
            new ProductTypeDefinition
            {
                Type = "tv",
                DisplayName = "Tivi",
                RequiredProperties = ProductPropertyHelper.GetSpecificPropertyNames(typeof(Television)),
                CreateInstance = () => new Television()
            },
            new ProductTypeDefinition
            {
                Type = "airconditioner",
                DisplayName = "Điều hòa",
                RequiredProperties = ProductPropertyHelper.GetSpecificPropertyNames(typeof(AirConditioner)),
                CreateInstance = () => new AirConditioner()
            },
            new ProductTypeDefinition
            {
                Type = "washingmachine",
                DisplayName = "Máy giặt",
                RequiredProperties = ProductPropertyHelper.GetSpecificPropertyNames(typeof(WashingMachine)),
                CreateInstance = () => new WashingMachine()
            }
        };

        public ProductModel CreateProduct(string type)
        {
            var def = _productTypes.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (def == null)
                throw new ArgumentException($"Unsupported product type: {type}");
            return def.CreateInstance();
        }

        public bool ValidateProductType(string type)
        {
            return _productTypes.Any(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public string[] GetSupportedProductTypes()
        {
            return _productTypes.Select(x => x.Type).ToArray();
        }

        public Dictionary<string, string> GetTypeDisplayNames()
        {
            return _productTypes.ToDictionary(x => x.Type, x => x.DisplayName);
        }

        public Dictionary<string, string[]> GetRequiredPropertiesForType(string type)
        {
            var def = _productTypes.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (def == null)
                throw new ArgumentException($"Unsupported product type: {type}");
            return new Dictionary<string, string[]> { { def.Type, def.RequiredProperties } };
        }

        public bool ValidateProductProperties(string type, Dictionary<string, object> properties)
        {
            var def = _productTypes.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            if (def == null)
                return false;
            return def.RequiredProperties.All(prop => properties != null && properties.ContainsKey(prop));
        }
    }
} 