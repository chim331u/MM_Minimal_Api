using MoneyManagement.Models.Balance;

namespace MoneyManagement.Interfaces
{
    public interface IBalanceService
    {
        Task<ICollection<Balance>> GetActiveBalanceList();
        Task<Balance> GetBalance(int balanceId);
        Task<Balance> AddBalance(Balance balance);
        Task<Balance> UpdateBalance(Balance balance);
        Task<Balance> DeleteBalance(Balance balance);
    }
}
