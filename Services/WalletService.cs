using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetUserBalanceAsync(string userId)
        {
            return await _unitOfWork.Wallets.GetUserBalanceAsync(userId);
        }

        public async Task<bool> UpdateUserBalanceAsync(string userId, decimal newBalance)
        {
            return await _unitOfWork.Wallets.UpdateUserBalanceAsync(userId, newBalance);
        }

        public async Task<bool> DeductFromBalanceAsync(string userId, decimal amount)
        {
            return await _unitOfWork.Wallets.DeductFromBalanceAsync(userId, amount);
        }

        public async Task<bool> AddToBalanceAsync(string userId, decimal amount)
        {
            return await _unitOfWork.Wallets.AddToBalanceAsync(userId, amount);
        }

        public async Task<bool> HasSufficientBalanceAsync(string userId, decimal amount)
        {
            return await _unitOfWork.Wallets.HasSufficientBalanceAsync(userId, amount);
        }
    }
} 