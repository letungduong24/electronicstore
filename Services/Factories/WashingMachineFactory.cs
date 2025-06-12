using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Factories
{
    public class WashingMachineFactory : IProductFactory
    {
        public ProductModel CreateProduct()
        {
            return new WashingMachine();
        }
    }
} 