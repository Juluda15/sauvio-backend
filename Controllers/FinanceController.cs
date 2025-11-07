using Microsoft.AspNetCore.Mvc;
using Sauvio.Services.Finance;
using Sauvio.Dto;

namespace Sauvio.Controllers
{
    [ApiController]
    [Route("api/finance")]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpPost("income")]
        public async Task<IActionResult> AddIncome([FromBody] TransactionDTO dto)
        {
            var result = await _financeService.AddIncome(dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("expense")]
        public async Task<IActionResult> AddExpense([FromBody] TransactionDTO dto)
        {
            var result = await _financeService.AddExpense(dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var result = await _financeService.GetBalance(userId);
            return Ok(result);
        }

    }
}
