using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IDbContextTransaction _transaction;
        
        private IUserRepository _users;
        private IProductRepository _products;
        private ICartRepository _carts;
        private IOrderRepository _orders;
        private IWalletRepository _wallets;

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IUserRepository Users => _users ??= new UserRepository(_userManager, _context);
        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public ICartRepository Carts => _carts ??= new CartRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IWalletRepository Wallets => _wallets ??= new WalletRepository(_context, _userManager);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.CommitAsync();
                }
                catch
                {
                    await _transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.RollbackAsync();
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
} 