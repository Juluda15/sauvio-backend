using Microsoft.EntityFrameworkCore;
using Sauvio.Data;
using Sauvio.Dto;
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
            if (dto.Amount <= 0 || !Regex.IsMatch(dto.Amount.ToString(), @"^\d+(\.\d{1,2})?$"))
                return (false, "Invalid amount. Must be positive and have at most 2 decimal places.");

            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return (false, "User not found.");

            user.TotalIncome += dto.Amount;
            user.Balance += dto.Amount;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return (true, $"Income of {dto.Amount:C} added successfully. New balance: {user.Balance:C}");
        }

        public async Task<(bool Success, string Message)> AddExpense(TransactionDTO dto)
        {
            if (dto.Amount <= 0 || !Regex.IsMatch(dto.Amount.ToString(), @"^\d+(\.\d{1,2})?$"))
                return (false, "Invalid amount. Must be positive and have at most 2 decimal places.");

            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return (false, "User not found.");

            if (user.Balance < dto.Amount)
                return (false, "Insufficient funds.");

            user.TotalExpense += dto.Amount;
            user.Balance -= dto.Amount;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return (true, $"Expense of {dto.Amount:C} recorded successfully. New balance: {user.Balance:C}");
        }

        public async Task<object> GetBalance(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                return new { Error = "User not found" };

            return new
            {
                user.Name,
                user.Email,
                user.TotalIncome,
                user.TotalExpense,
                user.Balance
            };
        }
    }
}
