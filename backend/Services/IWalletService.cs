using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services
{
    public interface IWalletService
    {
        Task<decimal> GetUserBalanceAsync(string userId);
        Task<bool> UpdateUserBalanceAsync(string userId, decimal newBalance);
        Task<bool> DeductFromBalanceAsync(string userId, decimal amount);
        Task<bool> AddToBalanceAsync(string userId, decimal amount);
        Task<bool> HasSufficientBalanceAsync(string userId, decimal amount);
    }
} 