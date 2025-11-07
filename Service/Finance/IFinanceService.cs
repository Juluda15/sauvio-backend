using Sauvio.Dto;

namespace Sauvio.Services.Finance
{
    public interface IFinanceService
    {
        Task<(bool Success, string Message)> AddIncome(TransactionDTO dto);
        Task<(bool Success, string Message)> AddExpense(TransactionDTO dto);
        Task<object> GetBalance(int userId);
    }
}
