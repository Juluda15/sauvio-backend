using Microsoft.AspNetCore.Mvc;
using Sauvio.Business.Dto;
using Sauvio.Business.Services.Account;
using Sauvio.Business.Exceptions;

namespace Sauvio.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                await _accountService.Register(dto);
                return Ok(new { message = "Registration successful. Please check your email." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var (token, user) = await _accountService.Login(dto);
                return Ok(new { token, user });
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            try
            {
                await _accountService.ConfirmEmail(token);
                return Ok(new { message = "Email confirmed successfully!" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                await _accountService.ChangePassword(dto);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
