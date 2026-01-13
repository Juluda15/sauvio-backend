using Microsoft.AspNetCore.Mvc;
using Sauvio.Business.Services.Finance;
using Sauvio.Business.Dto;
using Sauvio.Business.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Sauvio.Controllers
{
    [Authorize]
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
            try
            {
                if (dto.Type == "income")
                {
                    await _finance.AddIncome(dto);
                    return Ok(new { message = "Income added successfully" });
                }
                else if (dto.Type == "expense")
                {
                    await _finance.AddExpense(dto);
                    return Ok(new { message = "Expense added successfully" });
                }

                return BadRequest(new { message = "Invalid transaction type. Must be 'income' or 'expense'." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("expenses/{userId}")]
        public async Task<IActionResult> GetExpenses(int userId)
        {
            try
            {
                var result = await _finance.GetExpenses(userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("incomes/{userId}")]
        public async Task<IActionResult> GetIncomes(int userId)
        {
            try
            {
                var result = await _finance.GetIncomes(userId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            try
            {
                var (balance, income, expense) = await _finance.GetBalance(userId);
                return Ok(new { balance, income, expense });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("transaction/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDTO dto)
        {
            try
            {
                await _finance.UpdateTransaction(id, dto);
                return Ok(new { message = "Transaction updated successfully" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("transaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _finance.DeleteTransaction(id);
                return Ok(new { message = "Transaction deleted successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/balance/{userId}")]
        public async Task<IActionResult> AdminGetBalance(int userId)
        {
            var (balance, income, expense) = await _finance.GetBalance(userId);
            return Ok(new { balance, income, expense });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/incomes/{userId}")]
        public async Task<IActionResult> AdminGetIncomes(int userId)
        {
            return Ok(await _finance.GetIncomes(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/expenses/{userId}")]
        public async Task<IActionResult> AdminGetExpenses(int userId)
        {
            return Ok(await _finance.GetExpenses(userId));
        }
    }
}
