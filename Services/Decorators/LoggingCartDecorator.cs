using UserManagementAPI.DTOs;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Services.Decorators
{
    public class LoggingCartDecorator : BaseCartServiceDecorator
    {
        private readonly ILogger<LoggingCartDecorator> _logger;

        public LoggingCartDecorator(ICartService cartService, ILogger<LoggingCartDecorator> logger) 
            : base(cartService)
        {
            _logger = logger;
        }

        public override async Task<CartDto> GetUserCartAsync(string userId)
        {
            _logger.LogInformation("Getting cart for user: {UserId}", userId);
            try
            {
                var result = await base.GetUserCartAsync(userId);
                _logger.LogInformation("Successfully retrieved cart for user: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user: {UserId}", userId);
                throw;
            }
        }

        public override async Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            _logger.LogInformation("Adding product {ProductId} with quantity {Quantity} to cart for user: {UserId}", 
                addToCartDto.ProductId, addToCartDto.Quantity, userId);
            
            try
            {
                var result = await base.AddToCartAsync(userId, addToCartDto);
                _logger.LogInformation("Successfully added product {ProductId} to cart for user: {UserId}", 
                    addToCartDto.ProductId, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product {ProductId} to cart for user: {UserId}", 
                    addToCartDto.ProductId, userId);
                throw;
            }
        }

        public override async Task<CartDto> UpdateCartItemAsync(string userId, int cartItemId, UpdateCartItemDto updateDto)
        {
            _logger.LogInformation("Updating cart item {CartItemId} with quantity {Quantity} for user: {UserId}", 
                cartItemId, updateDto.Quantity, userId);
            
            try
            {
                var result = await base.UpdateCartItemAsync(userId, cartItemId, updateDto);
                _logger.LogInformation("Successfully updated cart item {CartItemId} for user: {UserId}", 
                    cartItemId, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {CartItemId} for user: {UserId}", 
                    cartItemId, userId);
                throw;
            }
        }

        public override async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
        {
            _logger.LogInformation("Removing cart item {CartItemId} for user: {UserId}", cartItemId, userId);
            
            try
            {
                var result = await base.RemoveFromCartAsync(userId, cartItemId);
                _logger.LogInformation("Successfully removed cart item {CartItemId} for user: {UserId}", 
                    cartItemId, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {CartItemId} for user: {UserId}", 
                    cartItemId, userId);
                throw;
            }
        }

        public override async Task<bool> ClearCartAsync(string userId)
        {
            _logger.LogInformation("Clearing cart for user: {UserId}", userId);
            
            try
            {
                var result = await base.ClearCartAsync(userId);
                _logger.LogInformation("Successfully cleared cart for user: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user: {UserId}", userId);
                throw;
            }
        }
    }
} 