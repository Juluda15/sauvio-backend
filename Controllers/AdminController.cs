using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sauvio.Business.Exceptions;
using Sauvio.Business.Services.Account;
using Sauvio.Business.Services.Finance;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IFinanceService _financeService;

    public AdminController(IAccountService accountService, IFinanceService financeService)
    {
        _accountService = accountService;
        _financeService = financeService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _accountService.GetAllUsers();
            var result = users.Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                balance = u.Balance
            });
            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new { message = "Failed to fetch users" });
        }
    }

    [HttpGet("balance/{userId}")]
    public async Task<IActionResult> GetUserBalance(int userId)
    {
        try
        {
            var (balance, income, expense) = await _financeService.GetBalance(userId);
            return Ok(new { balance, income, expense });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("incomes/{userId}")]
    public async Task<IActionResult> GetUserIncomes(int userId)
    {
        try
        {
            var incomes = await _financeService.GetIncomes(userId);
            return Ok(incomes);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("expenses/{userId}")]
    public async Task<IActionResult> GetUserExpenses(int userId)
    {
        try
        {
            var expenses = await _financeService.GetExpenses(userId);
            return Ok(expenses);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("promote/{userId}")]
    public async Task<IActionResult> PromoteToAdmin(int userId)
    {
        try
        {
            await _accountService.PromoteToAdmin(userId);
            return Ok(new { message = "User has been promoted to Admin" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var user = await _accountService.GetUserById(userId);
            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                isAdmin = user.IsAdmin,
                balance = user.Balance,
                totalIncome = user.TotalIncome,
                totalExpense = user.TotalExpense
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
