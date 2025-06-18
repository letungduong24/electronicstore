using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using UserManagementAPI.Mapper;

namespace UserManagementAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly CartMapper _cartMapper;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _cartMapper = new CartMapper();
        }

        public async Task<CartDto> GetUserCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            return _cartMapper.ToCartDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }

            await _cartRepository.AddItemToCartAsync(cart.Id, addToCartDto.ProductId, addToCartDto.Quantity);
            
            // Reload cart with updated items
            cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return _cartMapper.ToCartDto(cart);
        }

        public async Task<CartDto> UpdateCartItemAsync(string userId, int cartItemId, UpdateCartItemDto updateDto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new InvalidOperationException("Cart not found");

            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new InvalidOperationException("Cart item not found");

            await _cartRepository.UpdateCartItemQuantityAsync(cartItemId, updateDto.Quantity);
            
            // Reload cart with updated items
            cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return _cartMapper.ToCartDto(cart);
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return false;

            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null || cartItem.CartId != cart.Id)
                return false;

            return await _cartRepository.RemoveCartItemAsync(cartItemId);
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return false;

            return await _cartRepository.ClearCartAsync(cart.Id);
        }
    }
} 