using System;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Factories
{
    public class ProductFactory : IProductFactory
    {
        public ProductModel CreateProduct(string type)
        {
            return type.ToLower() switch
            {
                "tv" => new Television(),
                "airconditioner" => new AirConditioner(),
                "washingmachine" => new WashingMachine(),
                _ => throw new ArgumentException("Unknown product type")
            };
        }
    }
} 