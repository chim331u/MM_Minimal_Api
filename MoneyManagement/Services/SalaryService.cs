using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Salary;

namespace MoneyManagement.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ApplicationContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IAncillaryService _anchillaryService;
        private readonly ILogger<SalaryService> _logger;

        public SalaryService(ILogger<SalaryService> logger, ApplicationContext context, IUtilityService utilityService,
            IAncillaryService anchillaryService)
        {
            _context = context;
            _utilityService = utilityService;
            _anchillaryService = anchillaryService;
            _logger = logger;
        }

        #region Salary

        public async Task<ICollection<Salary>> GetActiveSalaryList()
        {
            try
            {
                var result = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.IsActive).OrderByDescending(x => x.SalaryDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching active salary list: {ex.Message}");
                return null;
            }
        }

        public async Task<Salary> GetSalary(int salaryId)
        {
            try
            {
                var result = await _context.Salary.Include(c => c.Currency).Include(c => c.User)
                    .Where(x => x.Id == salaryId).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching salary with ID {salaryId}: {ex.Message}");
                return null;
            }
        }

        public async Task<Salary> UpdateSalary(Salary item)
        {
            try
            {
                var existingSalary = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();

                if (existingSalary == null)
                {
                    _logger.LogWarning($"Salary with ID {item.Id} not found for update.");
                    return null;
                }

                existingSalary.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;
                existingSalary.LastUpdatedDate = DateTime.Now;
                existingSalary.ExcengeRate = item.ExcengeRate;
                existingSalary.SalaryValue = item.SalaryValue;
                existingSalary.SalaryDate = item.SalaryDate;
                existingSalary.FileName = item.FileName;
                existingSalary.ReferMonth = item.ReferMonth;
                existingSalary.ReferYear = item.ReferYear;
                existingSalary.Note = item.Note;

                _context.Salary.Update(existingSalary);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating salary: {ex.Message}");
                return null;
            }
        }

        public async Task<Salary> AddSalary(Salary item)
        {
            try
            {
                var currency = await _anchillaryService.GetCurrency(item.Currency.Id);
                var user = await _anchillaryService.GetServiceUser(item.User.Id);
                var currRate = await _anchillaryService.GetCurrencyRate(item.Currency.CurrencyCodeALF3);

                item.ExcengeRate = 1;
                if (currRate != null)
                {
                    item.ExcengeRate = currRate.RateValue;
                }

                item.Currency = currency;
                item.User = user;
                item.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;

                item.LastUpdatedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                await _context.Salary.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding salary: {ex.Message}");
                return null;
            }
        }

        public async Task<Salary> DeleteSalary(Salary item)
        {
            try
            {
                var existingSalary = await _context.Salary
                    .Include(c => c.Currency)
                    .Include(c => c.User)
                    .Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                if (existingSalary == null)
                {
                    _logger.LogWarning($"Salary with ID {item.Id} not found for deletion.");
                    return null;
                }
                
                existingSalary.LastUpdatedDate = DateTime.Now;
                existingSalary.IsActive = false;

                _context.Salary.Update(existingSalary);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting salary: {ex.Message}");
                return null;
            }
        }

        #endregion
    }
}