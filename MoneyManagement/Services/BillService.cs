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
                var result = await _context.bills.Include(c => c.Supplier)
                    .Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();
                if (result == null || result.Count == 0)
                {
                    _logger.LogWarning("No active bills found.");
                    return new List<Bill>();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Bill list {ex.Message}");
                return null;
            }
        }

        public async Task<Bill> GetBill(int billId)
        {
            try
            {
                var result = await _context.bills.Include(c => c.Supplier).Where(x => x.Id == billId)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Bill {ex.Message}");
                return null;
            }
        }

        public async Task<Bill> UpdateBill(Bill item)
        {
            try
            {
                var existingBill = await _context.bills.Include(x => x.Supplier).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();
                if (existingBill == null)
                {
                    _logger.LogWarning("Bill not found for update.");
                    return null;
                }

                existingBill.Amount = item.Amount;
                existingBill.BillNumber = item.BillNumber;
                existingBill.Consumption = item.Consumption;
                existingBill.FullPathFileName = item.FullPathFileName;
                existingBill.IsActive = item.IsActive;
                existingBill.PaidDate = item.PaidDate;
                existingBill.RefPeriodEnd = item.RefPeriodEnd;
                existingBill.RefPeriodStart = item.RefPeriodStart;
                existingBill.Supplier = item.Supplier;
                existingBill.Note = item.Note;
                existingBill.LastUpdatedDate = DateTime.Now;

                _context.bills.Update(existingBill);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Bill {ex.Message}");
                return null;
            }
        }

        public async Task<Bill> AddBill(Bill item)
        {
            var supplier = await _context.suppliers.Where(x => x.Id == item.Supplier.Id).FirstOrDefaultAsync();

            if (_context.bills.Where(x => x.BillNumber == item.BillNumber).Any())
            {
                _logger.LogWarning("Bill already present.");
                return null;
            }

            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Supplier = supplier;
                item.LastUpdatedDate = DateTime.Now;
                await _context.bills.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Bill {ex.Message}");
                return null;
            }
        }

        public async Task<Bill> DeleteBill(Bill item)
        {
            try
            {
                var existingBill = await _context.bills.Include(x => x.Supplier).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();

                if (existingBill == null)
                {
                    _logger.LogWarning("Bill not found for deletion.");
                    return null;
                }

                existingBill.LastUpdatedDate = DateTime.Now;
                existingBill.IsActive = false;

                _context.bills.Update(existingBill);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Bill {ex.Message}");
                return null;
            }
        }
    }
}