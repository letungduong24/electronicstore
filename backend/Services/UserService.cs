using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using UserManagementAPI.Mapper;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly UserMapper _userMapper;
        private readonly ICartRepository _cartRepository;
        private readonly IWalletService _walletService;

        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ICartRepository cartRepository,
            IWalletService walletService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _cartRepository = cartRepository;
            _userMapper = new UserMapper();
            _walletService = walletService;
        }

        public async Task<(bool success, string message, UserDto? user)> RegisterAsync(RegisterDto model)
        {
            var user = _userMapper.ToModel(model);
            if (user == null)
                return (false, "Invalid registration data", null);

            await _userRepository.AddAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);

            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null);

            // Assign default "User" role
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                // If role assignment fails, delete the user and return error
                await _userRepository.DeleteAsync(user);
                return (false, "Failed to assign default role", null);
            }

            // Create cart for the new user
            try
            {
                await _cartRepository.CreateCartAsync(user.Id);
            }
            catch (Exception ex)
            {
                // If cart creation fails, log the error but don't fail the registration
                // You might want to add proper logging here
                Console.WriteLine($"Failed to create cart for user {user.Id}: {ex.Message}");
            }

            var userDto = _userMapper.ToDTO(user);
            userDto.Balance = await _walletService.GetUserBalanceAsync(user.Id);
            return (true, "User registered successfully", userDto);
        }

        public async Task<(bool success, string message, UserDto? user)> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (false, "Invalid credentials", null);

            var roles = await _userRepository.GetUserRolesAsync(user);
            var userDto = _userMapper.ToDTO(user, roles.ToList());
            userDto.Balance = await _walletService.GetUserBalanceAsync(user.Id);
            return (true, "Login successful", userDto);
        }

        public async Task<(bool success, string message)> AssignRoleAsync(AssignRoleDto model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, "User not found");

            if (!await _roleManager.RoleExistsAsync(model.Role))
                return (false, "Role does not exist");

            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            
            // Remove all existing roles
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return (false, "Failed to remove existing roles");
            }

            // Add new role
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            return (result.Succeeded, result.Succeeded ? "Role assigned successfully" : string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool success, string message)> ChangePasswordAsync(string userId, ChangePasswordDto model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return (result.Succeeded, result.Succeeded ? "Password changed successfully" : string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool success, string message, UserDto? user)> UpdateUserAsync(string userId, UpdateUserDto model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found", null);

            _userMapper.UpdateModel(model, user);
            await _userRepository.UpdateAsync(user);
            var roles = await _userRepository.GetUserRolesAsync(user);
            var userDto = _userMapper.ToDTO(user, roles.ToList());
            userDto.Balance = await _walletService.GetUserBalanceAsync(user.Id);
            return (true, "User updated successfully", userDto);
        }

        public async Task<(bool success, string message)> DeleteUserAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return (false, "User not found");

            if (!await CanDeleteUserAsync(user))
                return (false, "Cannot delete admin account");

            await _userRepository.DeleteAsync(user);
            return (true, "User deleted successfully");
        }

        public async Task<(bool success, string message)> DeleteOwnAccountAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            await _userRepository.DeleteAsync(user);
            return (true, "Account deleted successfully");
        }

        public async Task<UserDto?> GetCurrentUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;
            // Lấy số lượng sản phẩm trong giỏ hàng
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            int cartItemCount = 0;
            if (cart != null && cart.CartItems != null)
            {
                cartItemCount = cart.CartItems.Sum(item => item.Quantity);
            }
            var roles = await _userRepository.GetUserRolesAsync(user);
            var userDto = _userMapper.ToDTO(user, roles.ToList(), cartItemCount);
            userDto.Balance = await _walletService.GetUserBalanceAsync(user.Id);
            return userDto;
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userRepository.GetUserRolesAsync(user);
                var userDto = _userMapper.ToDTO(user);
                userDtos.Add(new
                {
                    userDto.Id,
                    userDto.Email,
                    userDto.Name,
                    userDto.Address,
                    Roles = roles
                });
            }

            return userDtos;
        }

        public async Task<bool> CanDeleteUserAsync(ApplicationUser user)
        {
            return !await _userRepository.IsInRoleAsync(user, "Admin");
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userRepository.GetUserRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured")));
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 