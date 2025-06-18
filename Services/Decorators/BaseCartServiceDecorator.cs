using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services.Decorators
{
    public abstract class BaseCartServiceDecorator : ICartServiceDecorator
    {
        public ICartService CartService { get; }

        protected BaseCartServiceDecorator(ICartService cartService)
        {
            CartService = cartService;
        }

        public virtual async Task<CartDto> GetUserCartAsync(string userId)
        {
            return await CartService.GetUserCartAsync(userId);
        }

        public virtual async Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            return await CartService.AddToCartAsync(userId, addToCartDto);
        }

        public virtual async Task<CartDto> UpdateCartItemAsync(string userId, int cartItemId, UpdateCartItemDto updateDto)
        {
            return await CartService.UpdateCartItemAsync(userId, cartItemId, updateDto);
        }

        public virtual async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
        {
            return await CartService.RemoveFromCartAsync(userId, cartItemId);
        }

        public virtual async Task<bool> ClearCartAsync(string userId)
        {
            return await CartService.ClearCartAsync(userId);
        }
    }
} 