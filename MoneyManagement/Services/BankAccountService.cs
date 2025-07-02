using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Services
{
    public class BankAccountService : IBankAccountService
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<BankAccountService> _logger;

        public BankAccountService(ILogger<BankAccountService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;

        }

        #region Bank

        public async Task<ICollection<BankMasterData>?> GetActiveBankList()
        {
            try
            {
                var result = await _context.BankMasterData.Include(x=>x.Country)
                    .Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();

              return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve bank active list: {ex.Message}");
                return null;
            }
        }

        public async Task<BankMasterData?> GetBank(int bankId)
        {
            try
            {
                var result = await _context.BankMasterData
                    .Include(x=>x.Country)
                    .Where(x=>x.Id == bankId).FirstOrDefaultAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve bank {bankId}: {ex.Message}");
                return null;
            }
        }

        public async Task<BankMasterData?> UpdateBank(BankMasterData? item)
        {
           
            try
            {
                var existingBank = await _context.BankMasterData.Include(x=>x.Country).Where(x=>x.Id==item.Id).FirstOrDefaultAsync();

                if (existingBank == null)
                {
                    _logger.LogWarning($"Bank to update not found");
                    return null;
                }
                
                existingBank.Address=item.Address;
                existingBank.City=item.City;
                existingBank.Name = item.Name;
                existingBank.Description = item.Description;
                existingBank.IsActive=item.IsActive;
                existingBank.Mail=item.Mail;
                existingBank.Phone=item.Phone;
                existingBank.ReferenceName=item.ReferenceName;
                existingBank.WebUrl=item.WebUrl;
                existingBank.LastUpdatedDate = DateTime.Now;
                existingBank.Note=item.Note;

                _context.BankMasterData.Update(existingBank);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Updating bank: {ex.Message}");
                return null;

            }

        }

        public async Task<BankMasterData?> AddBank(BankMasterData? item)
        {
            var country = await _context.Country.FindAsync(item.Country.Id);
           
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Country = country;

                await _context.BankMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding bank: {ex.Message}");
                return null;

            }

        }

        public async Task<BankMasterData?> DeleteBank(BankMasterData? item)
        {
            
            try
            {
                var existingBank = await _context.BankMasterData.Include(x=>x.Country).Where(x=> x.Id == item.Id).FirstOrDefaultAsync();

                if (existingBank == null)
                {
                    _logger.LogWarning($"Bank not found");
                    return null;
                }

                existingBank.LastUpdatedDate = DateTime.Now;
                existingBank.IsActive = false;

                _context.BankMasterData.Update(existingBank);

                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting bank: {ex.Message}");
                return null;

            }

        }

        #endregion

        #region Account

        public async Task<ICollection<AccountMasterData>?> GetActiveAccountList()
        {
            try
            {
                var result = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.CreatedDate).ToListAsync();
                
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve Account list: {ex.Message}");
                return null;
            }
        }

        public async Task<AccountMasterData?> GetAccount(int accountId)
        {
            try
            {
                var result = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData)
                    .Where(x=>x.Id==accountId).FirstOrDefaultAsync();
                
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieve Account {accountId}: {ex.Message}");
                return null;
            }
        }

        public async Task<AccountMasterData?> UpdateAccount(AccountMasterData? item)
        {
            try
            {
                var existingAccount = await _context.AccountMasterData
                    .Include(x=>x.Currency)
                    .Include(x=>x.BankMasterData).Where(x=>x.Id==item.Id).FirstOrDefaultAsync();

                if (existingAccount == null)
                {
                    _logger.LogWarning($"Account to update not found");
                    return null;
                }
                
                existingAccount.AccountType = item.AccountType;
                existingAccount.LastUpdatedDate = DateTime.Now;

                _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<AccountMasterData?> AddAccount(AccountMasterData? item)
        {
            if (item == null)
            {
                _logger.LogWarning($"Account to add not found");
                return null;

            }

            if (item.Currency == null)
            {
                _logger.LogWarning($"Currency for Account to add not found");
                return null;
                    
            }

            var currency = await _context.Currency.FindAsync(item.Currency.Id);
            if (item.BankMasterData == null)
            {
                _logger.LogWarning($"Bank for Account to add not found");
                return null;
            }

            var bank = await _context.BankMasterData.FindAsync(item.BankMasterData.Id);

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.BankMasterData = bank;
                item.Currency = currency;

                await _context.AccountMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding new account : {ex.Message}");
                return null;
            }
        }

        public async Task<AccountMasterData?> DeleteAccount(AccountMasterData? item)
        {
            try
            {
                if (item == null)
                {
                    _logger.LogWarning($"Account to delete not found");
                    return null;
                }

                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting new account : {ex.Message}");
                return null;

            }

        }
        #endregion

    }
}