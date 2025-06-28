using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Interfaces
{
    public interface IBankAccountService
    {

        Task<ICollection<AccountMasterData>> GetActiveAccountList();
        Task<AccountMasterData> GetAccount(int accountId);
        Task<AccountMasterData> AddAccount(AccountMasterData account);
        Task<AccountMasterData> UpdateAccount(AccountMasterData account);
        Task<AccountMasterData> DeleteAccount(AccountMasterData account);

        //Task<ICollection<BankMasterData>> GetActiveBankList();
        Task<ICollection<BankMasterData>> GetActiveBankList();
        Task<BankMasterData> GetBank(int bankId);
        Task<BankMasterData> AddBank(BankMasterData bank);
        Task<BankMasterData> UpdateBank(BankMasterData bank);
        Task<BankMasterData> DeleteBank(BankMasterData bank);
    }
}
