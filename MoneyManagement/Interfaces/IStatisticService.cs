using MoneyManagement.Models.Statistics;

namespace MoneyManagement.Interfaces
{
    public interface IStatisticService
    {
        Task<Dashboard> GetDashboard();
        Task<List<SalaryStatistics>> GetSalaryStatistic(int userId);
    }
}
