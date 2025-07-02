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

        public async Task<ICollection<Balance>?> GetActiveBalanceList()
        {
            try
            {
                var result = await _context.Balance
                    .Include(c => c.Account)
                    .Include(c => c.Account.Currency)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.DateBalance).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Active balance list: {ex.Message}");
                return null;
            }
        }

        public async Task<Balance?> GetBalance(int balanceId)
        {
            try
            {
                var result = await _context.Balance
                    .Include(c => c.Account)
                    .Include(c => c.Account.Currency)
                    .Where(x => x.Id == balanceId).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Balance: {ex.Message}");
                return null;
            }
        }

        public async Task<Balance?> UpdateBalance(Balance item)
        {
            try
            {
                var existingBalance = await _context.Balance
                    .Include(c => c.Account)
                    .Include(c => c.Account.Currency)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();

                if (existingBalance == null)
                {
                    _logger.LogError("Balance not found for update.");
                    return null;
                }

                existingBalance.DateBalance = item.DateBalance;
                existingBalance.BalanceValue = item.BalanceValue;
                existingBalance.IsActive = item.IsActive;
                existingBalance.Note = item.Note;
                existingBalance.LastUpdatedDate = item.LastUpdatedDate;

                _context.Balance.Update(existingBalance);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error update Balance: {ex.Message}");
                return null;
            }
        }

        public async Task<Balance?> AddBalance(Balance item)
        {
            var account = await _context.AccountMasterData
                .Include(x => x.Currency)
                .Where(x => x.Id == item.Account.Id)
                .FirstOrDefaultAsync();

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Account = account;
                item.LastUpdatedDate = DateTime.Now;
                await _context.Balance.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Balance: {ex.Message}");
                return null;
            }
        }

        public async Task<Balance?> DeleteBalance(Balance item)
        {
            try
            {
                var existingBalance = await _context.Balance.Include(c => c.Account)
                    .Include(c => c.Account.Currency)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();

                if (existingBalance == null)
                {
                    _logger.LogWarning("Balance not found for delete.");
                    return null;
                }

                existingBalance.LastUpdatedDate = DateTime.Now;
                existingBalance.IsActive = false;

                _context.Balance.Update(existingBalance);

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