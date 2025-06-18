using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetMyBalance()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var balance = await _walletService.GetUserBalanceAsync(userId);
            return Ok(new { Balance = balance });
        }

        [HttpGet("balance/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<decimal>> GetUserBalance(string userId)
        {
            var balance = await _walletService.GetUserBalanceAsync(userId);
            return Ok(new { UserId = userId, Balance = balance });
        }

        [HttpPut("update-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUserBalance([FromBody] UpdateBalanceDto updateBalanceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _walletService.UpdateUserBalanceAsync(updateBalanceDto.UserId, updateBalanceDto.NewBalance);
            
            if (success)
            {
                return Ok(new { 
                    Message = "Balance updated successfully",
                    UserId = updateBalanceDto.UserId,
                    NewBalance = updateBalanceDto.NewBalance,
                    Reason = updateBalanceDto.Reason
                });
            }
            else
            {
                return BadRequest(new { Message = "Failed to update balance" });
            }
        }

        [HttpPost("add-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddToUserBalance([FromBody] AddBalanceDto addBalanceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _walletService.AddToBalanceAsync(addBalanceDto.UserId, addBalanceDto.Amount);
            
            if (success)
            {
                var newBalance = await _walletService.GetUserBalanceAsync(addBalanceDto.UserId);
                return Ok(new { 
                    Message = "Balance added successfully",
                    UserId = addBalanceDto.UserId,
                    AmountAdded = addBalanceDto.Amount,
                    NewBalance = newBalance,
                    Reason = addBalanceDto.Reason
                });
            }
            else
            {
                return BadRequest(new { Message = "Failed to add balance" });
            }
        }

        [HttpPost("deduct-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeductFromUserBalance([FromBody] DeductBalanceDto deductBalanceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _walletService.DeductFromBalanceAsync(deductBalanceDto.UserId, deductBalanceDto.Amount);
            
            if (success)
            {
                var newBalance = await _walletService.GetUserBalanceAsync(deductBalanceDto.UserId);
                return Ok(new { 
                    Message = "Balance deducted successfully",
                    UserId = deductBalanceDto.UserId,
                    AmountDeducted = deductBalanceDto.Amount,
                    NewBalance = newBalance,
                    Reason = deductBalanceDto.Reason
                });
            }
            else
            {
                return BadRequest(new { Message = "Failed to deduct balance - insufficient funds or user not found" });
            }
        }
    }

    public class AddBalanceDto
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
    }

    public class DeductBalanceDto
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
    }
} 