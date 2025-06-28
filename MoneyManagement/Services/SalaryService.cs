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

        public SalaryService(ILogger<SalaryService> logger, ApplicationContext context, IUtilityService utilityService, IAncillaryService anchillaryService)
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
                var result = await _context.Salary.Include(c => c.Currency).Include(c => c.User).Where(x => x.IsActive).OrderByDescending(x => x.SalaryDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Salary> GetSalary(int salaryId)
        {
            try
            {
                var result = await _context.Salary.Include(c => c.Currency).Include(c => c.User).Where(x=>x.Id==salaryId).FirstOrDefaultAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Salary> UpdateSalary(Salary item)
        {
            try
            {
                item.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.Salary.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                if (currRate!= null)
                {
                    item.ExcengeRate = currRate.RateValue;
                }

                item.Currency = currency;
                item.User = user;
                item.SalaryValueEur = item.SalaryValue / (double)item.ExcengeRate;
                
                item.LastUpdatedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.Salary.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Salary> DeleteSalary(Salary item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.Salary.Update(item);
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


    }
}
