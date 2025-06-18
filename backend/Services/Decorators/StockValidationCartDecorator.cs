using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services.Decorators
{
    public class StockValidationCartDecorator : BaseCartServiceDecorator
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public StockValidationCartDecorator(
            ICartService cartService, 
            IProductRepository productRepository,
            ICartRepository cartRepository) : base(cartService)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public override async Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            // Pre-validation: Check stock before adding to cart
            await ValidateStockForAddToCart(userId, addToCartDto);
            
            // Call the original service
            return await base.AddToCartAsync(userId, addToCartDto);
        }

        public override async Task<CartDto> UpdateCartItemAsync(string userId, int cartItemId, UpdateCartItemDto updateDto)
        {
            // Pre-validation: Check stock before updating cart item
            await ValidateStockForUpdateCartItem(userId, cartItemId, updateDto);
            
            // Call the original service
            return await base.UpdateCartItemAsync(userId, cartItemId, updateDto);
        }

        private async Task ValidateStockForAddToCart(string userId, AddToCartDto addToCartDto)
        {
            // Check if product exists and has sufficient stock
            var product = await _productRepository.GetProductByIdAsync(addToCartDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {addToCartDto.ProductId} not found");
            }

            if (product.Stock < addToCartDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Requested: {addToCartDto.Quantity}");
            }

            // Check if item already exists in cart and validate total quantity
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                var existingItem = await _cartRepository.GetCartItemByProductAsync(cart.Id, addToCartDto.ProductId);
                if (existingItem != null)
                {
                    var totalQuantity = existingItem.Quantity + addToCartDto.Quantity;
                    if (product.Stock < totalQuantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Total in cart: {totalQuantity}");
                    }
                }
            }
        }

        private async Task ValidateStockForUpdateCartItem(string userId, int cartItemId, UpdateCartItemDto updateDto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new InvalidOperationException("Cart not found");

            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new InvalidOperationException("Cart item not found");

            // Check if product has sufficient stock for the new quantity
            var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {cartItem.ProductId} not found");
            }

            if (product.Stock < updateDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Requested: {updateDto.Quantity}");
            }
        }
    }
} 