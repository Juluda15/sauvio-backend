using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sauvio.Dto;
using Sauvio.Services.Account;

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
            var result = await _accountService.Register(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var (success, message, user) = await _accountService.Login(dto);
            return success ? Ok(user) : BadRequest(message);
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var message = await _accountService.ConfirmEmail(token);

            if (message == "Email confirmed successfully!")
                return Ok(message);

            return BadRequest(message);
        }

    }

}
