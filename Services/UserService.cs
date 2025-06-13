using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterDto model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = model.Email;

            await _userRepository.AddAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);

            return (result.Succeeded, result.Succeeded ? "User registered successfully" : string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool success, string message, string? token)> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (false, "Invalid credentials", null);

            var token = await GenerateJwtTokenAsync(user);
            return (true, "Login successful", token);
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

        public async Task<(bool success, string message)> UpdateUserAsync(string userId, UpdateUserDto model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            _mapper.Map(model, user);
            await _userRepository.UpdateAsync(user);
            return (true, "User updated successfully");
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
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userRepository.GetUserRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);
                userDtos.Add(new
                {
                    userDto.Id,
                    userDto.Email,
                    userDto.Name,
                    userDto.Address,
                    userDto.Phone,
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