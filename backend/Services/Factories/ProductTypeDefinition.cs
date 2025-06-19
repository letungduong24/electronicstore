using UserManagementAPI.Models;
using System;

namespace UserManagementAPI.Services.Factories
{
    public class ProductTypeDefinition
    {
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string[] RequiredProperties { get; set; }
        public Func<ProductModel> CreateInstance { get; set; }
    }
} 