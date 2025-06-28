using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Bill;

namespace MoneyManagement.Services
{
    public class BillService : IBillService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BillService> _logger;

        public BillService(ApplicationContext context, ILogger<BillService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ICollection<Bill>> GetActiveBillList()
        {
            try
            {
                var result = await _context.bills.Include(c=>c.Supplier).Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();

                ICollection<Bill> billList = new List<Bill>();

                
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Bill> GetBill(int billId)
        {
            try
            {
                var result = await _context.bills.Include(c => c.Supplier).Where(x=>x.Id == billId).FirstOrDefaultAsync();

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Bill> UpdateBill(Bill item)
        {
            
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.bills.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Bill> AddBill(Bill item)
        {
            var supplier = await _context.suppliers.Where(x=>x.Id==item.Supplier.Id).FirstOrDefaultAsync();
            
            if(_context.bills.Where(x=>x.BillNumber == item.BillNumber).Any())
            {
                _logger.LogWarning("Bill already present.");
                return null;
            }

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Supplier= supplier;
                item.LastUpdatedDate= DateTime.Now;
                var result = await _context.bills.AddAsync(item);
                await _context.SaveChangesAsync();

                return item; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<Bill> DeleteBill(Bill item)
        {
           
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.bills.Update(item);
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
