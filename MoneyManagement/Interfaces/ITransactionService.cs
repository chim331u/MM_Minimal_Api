using MoneyManagement.Models.Transactions;

namespace MoneyManagement.Interfaces
{
    public interface ITransactionService
    {
        Task<ICollection<Transaction>> GetActiveTransactionList();
        Task<Transaction> GetTransaction(int transactionId);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<Transaction> CategoryConfirmed(Transaction transaction);
        Task<Transaction> CategorizeTransaction(Transaction transaction);
        Task<string> TrainModelTransaction();
        Task<string> CategorizeAllTransaction();
        Task<Transaction> DeleteTransaction(Transaction transaction);
        Task<string> UploadCsv(IList<Transaction> transactions);
    }
}
