using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services.Decorators
{
    public static class CartServiceDecoratorFactory
    {
        public static ICartService CreateDecoratedCartService(IServiceProvider serviceProvider)
        {
            // Get the base cart service
            var cartService = serviceProvider.GetRequiredService<CartService>();
            
            // Get required dependencies
            var productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            var cartRepository = serviceProvider.GetRequiredService<ICartRepository>();
            var logger = serviceProvider.GetRequiredService<ILogger<LoggingCartDecorator>>();
            
            // Chain decorators in order: Logging -> StockValidation -> CartService
            // The order matters: Logging will wrap StockValidation, which wraps CartService
            var stockValidationDecorator = new StockValidationCartDecorator(cartService, productRepository, cartRepository);
            var loggingDecorator = new LoggingCartDecorator(stockValidationDecorator, logger);
            
            return loggingDecorator;
        }
    }
} 