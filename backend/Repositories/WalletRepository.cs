using UserManagementAPI.Data;
using UserManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<decimal> GetUserBalanceAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Balance ?? 0;
        }

        public async Task<bool> UpdateUserBalanceAsync(string userId, decimal newBalance)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                user.Balance = newBalance;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeductFromBalanceAsync(string userId, decimal amount)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                if (user.Balance < amount)
                    return false;

                user.Balance -= amount;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddToBalanceAsync(string userId, decimal amount)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                user.Balance += amount;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasSufficientBalanceAsync(string userId, decimal amount)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Balance >= amount;
        }
    }
} 