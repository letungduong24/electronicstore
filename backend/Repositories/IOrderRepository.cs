using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> GetOrderByNumberAsync(string orderNumber);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<string> GenerateOrderNumberAsync();
    }
} 