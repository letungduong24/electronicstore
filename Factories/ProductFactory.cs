using ElectronicStore.Models;

namespace ElectronicStore.Factories;

public class ProductFactory : IProductFactory
{
    private readonly Dictionary<FactoryProductType, Func<ProductModel>> _productCreators;

    public ProductFactory()
    {
        _productCreators = new Dictionary<FactoryProductType, Func<ProductModel>>
        {
            { FactoryProductType.AirConditioner, () => new AirConditioner() },
            { FactoryProductType.WashingMachine, () => new WashingMachine() },
            { FactoryProductType.Television, () => new Television() }
        };
    }

    public ProductModel CreateProduct(FactoryProductType type)
    {
        if (_productCreators.TryGetValue(type, out var creator))
        {
            return creator();
        }
        throw new ArgumentException($"Invalid product type: {type}");
    }
} 