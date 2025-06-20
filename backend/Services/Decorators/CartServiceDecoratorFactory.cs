using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services.Decorators
{
    public static class CartServiceDecoratorFactory
    {
        public static ICartService CreateDecoratedCartService(IServiceProvider serviceProvider)
        {
            var cartService = serviceProvider.GetRequiredService<CartService>();
            
            var productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            var cartRepository = serviceProvider.GetRequiredService<ICartRepository>();
            var logger = serviceProvider.GetRequiredService<ILogger<LoggingCartDecorator>>();
            
            var stockValidationDecorator = new StockValidationCartDecorator(cartService, productRepository, cartRepository);
            var loggingDecorator = new LoggingCartDecorator(stockValidationDecorator, logger);
            
            return loggingDecorator;
        }
    }
} 