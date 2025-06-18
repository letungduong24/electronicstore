using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mapper
{
    public class CartMapper
    {
        public CartItemDto ToCartItemDto(CartItem cartItem)
        {
            if (cartItem == null) return null;

            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product?.Name ?? string.Empty,
                ProductImage = cartItem.Product?.ImageUrl ?? string.Empty,
                ProductPrice = cartItem.Product?.Price ?? 0,
                Quantity = cartItem.Quantity,
                TotalPrice = (cartItem.Product?.Price ?? 0) * cartItem.Quantity
            };
        }

        public CartDto ToCartDto(Cart cart)
        {
            if (cart == null) return null;

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = new List<CartItemDto>()
            };

            if (cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    cartDto.Items.Add(ToCartItemDto(item));
                }
            }

            cartDto.TotalItems = cartDto.Items.Sum(item => item.Quantity);
            cartDto.TotalPrice = cartDto.Items.Sum(item => item.TotalPrice);

            return cartDto;
        }
    }
} 