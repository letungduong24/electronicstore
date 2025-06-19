using System;
using System.Linq;

using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Factories
{
    public static class ProductPropertyHelper
    {
        public static string[] GetSpecificPropertyNames(Type productType)
        {
            var baseProps = typeof(ProductModel).GetProperties()
                                                .Select(p => p.Name)
                                                .ToHashSet();

            return productType.GetProperties()
                              .Where(p => !baseProps.Contains(p.Name))
                              .Select(p => p.Name)
                              .ToArray();
        }
    }
} 