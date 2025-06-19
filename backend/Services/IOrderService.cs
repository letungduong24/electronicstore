using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromCartAsync(string userId, CreateOrderDto createOrderDto);
        Task<OrderDto> GetOrderByIdAsync(int orderId, string userId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
        Task<OrderDto> CancelOrderAsync(int orderId, string userId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    }
} 