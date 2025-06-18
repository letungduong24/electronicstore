using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mapper
{
    public class OrderMapper
    {
        public OrderItemDto ToOrderItemDto(OrderItem orderItem)
        {
            if (orderItem == null) return null;

            return new OrderItemDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.ProductName,
                UnitPrice = orderItem.UnitPrice,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice
            };
        }

        public OrderDto ToOrderDto(Order order)
        {
            if (order == null) return null;

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentId = order.PaymentId,
                PayerId = order.PayerId,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                OrderItems = new List<OrderItemDto>()
            };

            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    orderDto.OrderItems.Add(ToOrderItemDto(item));
                }
            }

            return orderDto;
        }
    }
} 