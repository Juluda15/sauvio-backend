using Microsoft.AspNetCore.Mvc;
using Sauvio.Services.Finance;
using Sauvio.Dto;

namespace Sauvio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _finance;

        public FinanceController(IFinanceService finance)
        {
            _finance = finance;
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> AddTransaction(TransactionDTO dto)
        {
            if (dto.Type == "income")
            {
                var result = await _finance.AddIncome(dto);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            else if (dto.Type == "expense")
            {
                var result = await _finance.AddExpense(dto);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            return BadRequest("Invalid transaction type. Must be 'income' or 'expense'.");
        }

        [HttpGet("expenses/{userId}")]
        public async Task<IActionResult> GetExpenses(int userId)
        {
            var result = await _finance.GetExpenses(userId);
            return Ok(result);
        }

        [HttpGet("incomes/{userId}")]
        public async Task<IActionResult> GetIncomes(int userId)
        {
            var result = await _finance.GetIncomes(userId);
            return Ok(result);
        }

        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var user = await _finance.GetBalance(userId);
            return Ok(user);
        }
    }
}
