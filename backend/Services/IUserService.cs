using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<(bool success, string message, UserDto? user)> RegisterAsync(RegisterDto model);
        Task<(bool success, string message, UserDto? user)> LoginAsync(LoginDto model);
        Task<(bool success, string message)> AssignRoleAsync(AssignRoleDto model);
        Task<(bool success, string message)> ChangePasswordAsync(string userId, ChangePasswordDto model);
        Task<(bool success, string message, UserDto? user)> UpdateUserAsync(string userId, UpdateUserDto model);
        Task<(bool success, string message)> DeleteUserAsync(string email);
        Task<(bool success, string message)> DeleteOwnAccountAsync(string userId);
        Task<UserDto?> GetCurrentUserAsync(string userId);
        Task<IEnumerable<object>> GetAllUsersAsync();
        Task<bool> CanDeleteUserAsync(ApplicationUser user);
    }
} 