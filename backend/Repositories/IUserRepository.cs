using System.Linq.Expressions;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> expression);
        Task AddAsync(ApplicationUser entity);
        Task UpdateAsync(ApplicationUser entity);
        Task DeleteAsync(ApplicationUser entity);
        Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> expression);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    }
} 