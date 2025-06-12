using System;
using System.Collections.Generic;
using UserManagementAPI.Services.Factories;

namespace UserManagementAPI.Services
{
    public class ProductFactoryResolver
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<string, Type> _factoryTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "AirConditioner", typeof(AirConditionerFactory) },
            { "WashingMachine", typeof(WashingMachineFactory) },
            { "Television", typeof(TelevisionFactory) }
        };

        public ProductFactoryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProductFactory ResolveFactory(string productType)
        {
            if (_factoryTypes.TryGetValue(productType, out var factoryType))
            {
                return (IProductFactory)_serviceProvider.GetService(factoryType);
            }
            throw new ArgumentException($"No factory found for product type: {productType}");
        }
    }
} 