using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IWalletRepository
    {
        Task<decimal> GetUserBalanceAsync(string userId);
        Task<bool> UpdateUserBalanceAsync(string userId, decimal newBalance);
        Task<bool> DeductFromBalanceAsync(string userId, decimal amount);
        Task<bool> AddToBalanceAsync(string userId, decimal amount);
        Task<bool> HasSufficientBalanceAsync(string userId, decimal amount);
    }
} 