using ElectronicStore.DTOs;
using ElectronicStore.Models;

namespace ElectronicStore.Repositories
{
    public interface IAuthRepository
    {
        Task<UserModel> GetUserByUsername(string username);
        Task<bool> IsUsernameExists(string username);
        Task<bool> IsEmailExists(string email);
        Task<UserModel> CreateUser(UserModel user);
    }
} 