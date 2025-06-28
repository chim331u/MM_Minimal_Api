using MoneyManagement.Models.Bill;

namespace MoneyManagement.Interfaces
{
    public interface IBillService
    {
        Task<ICollection<Bill>> GetActiveBillList();
        Task<Bill> GetBill(int BillId);
        Task<Bill> AddBill(Bill bill);
        Task<Bill> UpdateBill(Bill bill);
        Task<Bill> DeleteBill(Bill bill);
    }
}
