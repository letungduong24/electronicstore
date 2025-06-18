using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateCartAsync(string userId);
        Task<CartItem> AddItemToCartAsync(int cartId, int productId, int quantity);
        Task<CartItem> UpdateCartItemQuantityAsync(int cartItemId, int quantity);
        Task<bool> RemoveCartItemAsync(int cartItemId);
        Task<bool> ClearCartAsync(int cartId);
        Task<CartItem> GetCartItemAsync(int cartItemId);
        Task<bool> CartItemExistsAsync(int cartId, int productId);
    }
} 