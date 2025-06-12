using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Factories
{
    public class AirConditionerFactory : IProductFactory
    {
        public ProductModel CreateProduct()
        {
            return new AirConditioner();
        }
    }
} 