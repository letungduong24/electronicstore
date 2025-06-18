using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _roleManager.RoleExistsAsync(model.Name))
                return BadRequest("Role already exists");

            var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (result.Succeeded)
                return Ok(new { message = "Role created successfully" });

            return BadRequest(result.Errors);
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound("Role not found");

            if (roleName == "Admin" || roleName == "User")
                return BadRequest("Cannot delete default roles");

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok(new { message = "Role deleted successfully" });

            return BadRequest(result.Errors);
        }

        [HttpGet("users/{roleName}")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                return NotFound("Role not found");

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
        }
    }

    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
} 