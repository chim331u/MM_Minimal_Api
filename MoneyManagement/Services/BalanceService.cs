using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Balance;

namespace MoneyManagement.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BalanceService> _logger;

        public BalanceService(ApplicationContext context, ILogger<BalanceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ICollection<Balance>> GetActiveBalanceList()
        {
            try
            {
                var result = await _context.Balance.Include(c=>c.Account).Include(c=>c.Account.Currency).Where(x => x.IsActive).OrderByDescending(x => x.DateBalance).ToListAsync();

                ICollection<Balance> balanceList = new List<Balance>();

                
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Balance> GetBalance(int balanceId)
        {
            try
            {
                var result = await _context.Balance.Include(c => c.Account).Include(c => c.Account.Currency).Where(x=>x.Id == balanceId).FirstOrDefaultAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Balance> UpdateBalance(Balance item)
        {
            
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.Balance.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Balance> AddBalance(Balance item)
        {
            var account = await _context.AccountMasterData.Include(x=>x.Currency).Where(x=>x.Id==item.Account.Id).FirstOrDefaultAsync();
            
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Account= account;
                item.LastUpdatedDate= DateTime.Now;
                var result = await _context.Balance.AddAsync(item);
                await _context.SaveChangesAsync();

                return item; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Balance> DeleteBalance(Balance item)
        {
           
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.Balance.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }


    }
}
