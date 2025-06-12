using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> AddToRoleAsync(ApplicationUser user, string role);
        Task<bool> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    }
} 