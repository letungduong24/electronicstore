using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Factories
{
    public class TelevisionFactory : IProductFactory
    {
        public ProductModel CreateProduct()
        {
            return new Television();
        }
    }
} 