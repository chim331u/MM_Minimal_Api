using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.IdentityAccess;

namespace MoneyManagement.Services
{
    public class IdentityAccessService : IIdentityAccessService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IAncillaryService _anchillaryService;
        private readonly ILogger<IdentityAccessService> _logger;

        public IdentityAccessService(ILogger<IdentityAccessService> logger, ApplicationContext context, IUtilityService utilityService, IAncillaryService anchillaryService)
        {
            _context = context;
            _utilityService = utilityService;
            _anchillaryService = anchillaryService;

            _logger = logger;
        }

        protected IdentityAccessService()
        {
            throw new NotImplementedException();
        }

        #region Identity Accounts
        public async Task<ICollection<ISA_Accounts>> GetActiveIdentityAccountList()
        {
            try
            {
                var result = await _context.ISA_Accounts.Include(c => c.ISA_User).Where(x => x.IsActive)
                    .OrderByDescending(x => x.CreatedDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ISA_Accounts> GetIdentityAccount(int isaAccountId)
        {
            try
            {
                var result = await _context.ISA_Accounts.Include(c => c.ISA_User).Where(x => x.Id == isaAccountId).FirstOrDefaultAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ISA_Accounts> UpdateIdentityAccount(ISA_Accounts item)
        {
            try
            {

                item.LastUpdatedDate = DateTime.Now;

                var result = _context.ISA_Accounts.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ISA_Accounts> AddIdentityAccount(ISA_Accounts item)
        {
            try
            {
                var person = await _anchillaryService.GetServiceUser(item.ISA_User.Id);


                if (person != null)
                {
                    item.ISA_User = person;
                }


                item.LastUpdatedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.ISA_Accounts.AddAsync(item);
                await _context.SaveChangesAsync();

                item.Password = _utilityService.EncryptString(item);
                _context.ISA_Accounts.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<ISA_Accounts> DeleteIdentityAccount(ISA_Accounts item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.ISA_Accounts.Update(item);
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

        #region password management

        public async Task<string> GetCleanPsw(int isa_account_id)
        {
            try
            {
                var account = await _context.ISA_Accounts.FindAsync(isa_account_id);

                if (account != null)
                {
                    return _utilityService.DecryptString(account);
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }
        }


        public async Task<string> PasswordChange(ISA_Accounts item)
        {
            var oldPassword = await _context.ISA_Accounts.Where(x => x.Id == item.Id).Select(x => x.Password).FirstOrDefaultAsync();


            item.LastUpdatedDate = DateTime.Now;
            item.Password = _utilityService.EncryptString(item);

            try
            {
                var result = _context.ISA_Accounts.Update(item);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

            var account = await _context.ISA_Accounts.Include(x => x.ISA_User).Where(x => x.Id == item.Id).FirstOrDefaultAsync();

            try
            {
                _context.ISA_PasswordsOld.Add(new ISA_PasswordsOld
                {
                    CreatedDate = item.CreatedDate,
                    IsActive = true,
                    LastUpdatedDate = DateTime.Now,
                    ISA_Account = account,
                    Password = oldPassword
                });
                await _context.SaveChangesAsync();
                return "Password Changed";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }

        }

        public async Task<ICollection<ISA_PasswordsOld>> ListAllOldPasswords(int accountId)
        {
            try
            {
                var result = await _context.ISA_PasswordsOld.Include(c => c.ISA_Account).Where(x => x.IsActive && x.ISA_Account.Id == accountId).OrderByDescending(x => x.LastUpdatedDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        #endregion

    }
}
