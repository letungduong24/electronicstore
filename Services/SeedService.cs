using Microsoft.AspNetCore.Identity;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Services
{
    public class SeedService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;

        public SeedService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ICartRepository cartRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _cartRepository = cartRepository;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        public async Task SeedAdminAsync()
        {
            var adminEmail = _configuration["AdminUser:Email"] ?? "admin@example.com";
            var adminPassword = _configuration["AdminUser:Password"] ?? "Admin123!";

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Tạo giỏ hàng cho admin nếu chưa có
            var cart = await _cartRepository.GetCartByUserIdAsync(adminUser.Id);
            if (cart == null)
            {
                await _cartRepository.CreateCartAsync(adminUser.Id);
            }
        }
    }
} 