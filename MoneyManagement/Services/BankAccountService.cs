using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<BankMasterData>> GetActiveBankList()
        {
            try
            {
                
                var result = await _context.BankMasterData.Include("Country")
                    .Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();

              return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<BankMasterData> GetBank(int bankId)
            {
            try
            {
                var result = await _context.BankMasterData.Include("Country").Where(x=>x.Id == bankId).FirstOrDefaultAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<BankMasterData> UpdateBank(BankMasterData item)
        {
           
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.BankMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<BankMasterData> AddBank(BankMasterData item)
        {
            var country = await _context.Country.FindAsync(item.Country.Id);
           
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Country = country;

                var result = await _context.BankMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<BankMasterData> DeleteBank(BankMasterData item)
        {
            
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.BankMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        #endregion

        #region Account

        public async Task<ICollection<AccountMasterData>> GetActiveAccountList()
        {
            try
            {
                var result = await _context.AccountMasterData.Include("Currency").Include("BankMasterData").Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();
                
                //ICollection<Account_DTO> accountList = new List<Account_DTO>();

                //foreach (var account in result)
                //{
                //    accountList.Add(await AccountFromDataToDTO(account));

                //}
                _logger.LogInformation($"TEST");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<AccountMasterData> GetAccount(int accountId)
        {
            try
            {
                var result = await _context.AccountMasterData.Include("Currency").Include("BankMasterData").Where(x=>x.Id==accountId).FirstOrDefaultAsync();
                
                if (result == null)
                { return null; }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<AccountMasterData> UpdateAccount(AccountMasterData item)
        {
            //var account = await AccountFromDTOToData(item);

            try
            {
                //account.LastUpdatedDate = DateTime.Now;

                //var result = _context.AccountMasterData.Update(account);
                //await _context.SaveChangesAsync();

                //return await AccountFromDataToDTO(account); 
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<AccountMasterData> AddAccount(AccountMasterData item)
        {
            var currency = await _context.Currency.FindAsync(item.Currency.Id);
            var bank = await _context.BankMasterData.FindAsync(item.BankMasterData.Id);
            //var account = await AccountFromDTOToData(item);

            //if (account == null)
            //{
            //    account = new AccountMasterData();
            //    account.CreatedDate = DateTime.Now;
            //}

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.BankMasterData = bank;
                item.Currency = currency;

                var result = await _context.AccountMasterData.AddAsync(item);
                await _context.SaveChangesAsync();

                return item; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<AccountMasterData> DeleteAccount(AccountMasterData item)
        {
            //var account = await AccountFromDTOToData(item);

            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.AccountMasterData.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        //private async Task<Account_DTO> AccountFromDataToDTO(AccountMasterData item)
        //{
        //    var result = new Account_DTO
        //    {
        //        Id = item.Id, AccountType = item.AccountType, Bic = item.Bic, Conto = item.Conto, Description = item.Description, 
        //        Iban = item.Iban, Name = item.Name, Note = item.Note, BankId = item.BankMasterData.Id, BankName = item.BankMasterData.Name,
        //        CreatedDate = item.CreatedDate, CurrencyId = item.Currency.Id, CurrencyName = item.Currency.Name, Currency=item.Currency
            
        //    };
        //    return result;
        //}

        //private async Task<AccountMasterData> AccountFromDTOToData(Account_DTO item)
        //{
        //    var currency = await _context.Currency.Where(x => x.Name == item.CurrencyName).FirstOrDefaultAsync();
        //    var bank = await _context.BankMasterData.Where(x => x.Name == item.BankName).FirstOrDefaultAsync();

        //    var accountMasterData = await _context.AccountMasterData.FindAsync(item.Id);

        //    if (accountMasterData == null)
        //    {
        //        accountMasterData = new AccountMasterData();
        //        accountMasterData.CreatedDate = DateTime.Now;
        //    }

        //    accountMasterData.Name = item.Name;
        //    accountMasterData.Conto = item.Conto;
        //    accountMasterData.Description = item.Description;
        //    accountMasterData.Iban = item.Iban;
        //    accountMasterData.Bic = item.Bic;
        //    accountMasterData.AccountType = item.AccountType;
        //    accountMasterData.CreatedDate = item.CreatedDate;
        //    accountMasterData.Note = item.Note;
        //    accountMasterData.Currency = currency;
        //    accountMasterData.BankMasterData = bank;


        //    return accountMasterData;


        //}

        #endregion

    }
}