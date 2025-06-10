using ElectronicStore.Models;

namespace ElectronicStore.Factories;

public interface IProductFactory
{
    ProductModel CreateProduct(FactoryProductType type);
} 