using Microsoft.EntityFrameworkCore;
using Sauvio.Data;
using Sauvio.Dto;
using Sauvio.Models.User;
using System.Text.RegularExpressions;

namespace Sauvio.Services.Finance
{
    public class FinanceService : IFinanceService
    {
        private readonly AppDbContext _db;

        public FinanceService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(bool Success, string Message)> AddIncome(TransactionDTO dto)
        {
            if (dto.Amount <= 0)
                return (false, "Amount must be positive.");

            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return (false, "User not found.");

            var transaction = new Transaction
            {
                UserId = dto.UserId,
                Amount = dto.Amount,
                Type = "income",
                Description = dto.Description,
                SourceOrCategory = dto.SourceOrCategory
            };

            _db.Transactions.Add(transaction);

            user.TotalIncome += dto.Amount;
            user.Balance += dto.Amount;

            await _db.SaveChangesAsync();

            return (true, "Income added successfully.");
        }

        public async Task<(bool Success, string Message)> AddExpense(TransactionDTO dto)
        {
            if (dto.Amount <= 0)
                return (false, "Amount must be positive.");

            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return (false, "User not found.");

            if (user.Balance < dto.Amount)
                return (false, "Insufficient balance.");

            var transaction = new Transaction
            {
                UserId = dto.UserId,
                Amount = dto.Amount,
                Type = "expense",
                Description = dto.Description,
                SourceOrCategory = dto.SourceOrCategory
            };

            _db.Transactions.Add(transaction);

            user.TotalExpense += dto.Amount;
            user.Balance -= dto.Amount;

            await _db.SaveChangesAsync();

            return (true, "Expense added successfully.");
        }

        public async Task<List<Transaction>> GetExpenses(int userId)
        {
            return await _db.Transactions
                .Where(t => t.UserId == userId && t.Type == "expense")
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetIncomes(int userId)
        {
            return await _db.Transactions
                .Where(t => t.UserId == userId && t.Type == "income")
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<object> GetBalance(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return new { Error = "User not found" };

            return new
            {
                user.Balance,
                user.TotalIncome,
                user.TotalExpense
            };
        }
    }
}
