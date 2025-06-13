using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> expression)
        {
            return await _userManager.Users.Where(expression).ToListAsync();
        }

        public async Task AddAsync(ApplicationUser entity)
        {
            await _userManager.CreateAsync(entity);
        }

        public async Task UpdateAsync(ApplicationUser entity)
        {
            await _userManager.UpdateAsync(entity);
        }

        public async Task DeleteAsync(ApplicationUser entity)
        {
            await _userManager.DeleteAsync(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
} 