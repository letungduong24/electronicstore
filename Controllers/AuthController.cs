using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElectronicStore.DTOs;
using ElectronicStore.Models;
using ElectronicStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ElectronicStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _authRepository.GetUserByUsername(request.Username);

            if (user == null)
            {
                return BadRequest(new AuthResponse { Success = false, Message = "User not found" });
            }

            // In a real application, you should use proper password hashing
            if (user.Password != request.Password)
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Invalid password" });
            }

            var token = GenerateJwtToken(user);

            // Set JWT token in cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            if (await _authRepository.IsUsernameExists(request.Username))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Username already exists" });
            }

            if (await _authRepository.IsEmailExists(request.Email))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Email already exists" });
            }

            var user = new UserModel
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password, // In a real application, hash the password
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Role = UserRole.User // Default role
            };

            user = await _authRepository.CreateUser(user);
            var token = GenerateJwtToken(user);

            // Set JWT token in cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Registration successful",
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        [HttpPost("register-admin")]
        public async Task<ActionResult<AuthResponse>> RegisterAdmin(RegisterRequest request)
        {
            if (await _authRepository.IsUsernameExists(request.Username))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Username already exists" });
            }

            if (await _authRepository.IsEmailExists(request.Email))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Email already exists" });
            }

            var user = new UserModel
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Role = UserRole.Admin
            };

            user = await _authRepository.CreateUser(user);
            var token = GenerateJwtToken(user);

            // Set JWT token in cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Admin registration successful",
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        [HttpPost("register-staff")]
        public async Task<ActionResult<AuthResponse>> RegisterStaff(RegisterRequest request)
        {
            if (await _authRepository.IsUsernameExists(request.Username))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Username already exists" });
            }

            if (await _authRepository.IsEmailExists(request.Email))
            {
                return BadRequest(new AuthResponse { Success = false, Message = "Email already exists" });
            }

            var user = new UserModel
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Role = UserRole.Staff
            };

            user = await _authRepository.CreateUser(user);
            var token = GenerateJwtToken(user);

            // Set JWT token in cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Staff registration successful",
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { Message = "Logged out successfully" });
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 