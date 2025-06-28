namespace MoneyManagement.Models.Statistics
{
    public class Dashboard
    {
        public IList<BalanceLineChart>? BalanceLineChar { get; set; }
        public IList<SpentChart>? SpentChar { get; set; }
        public IList<SalaryChart>? SalaryCharts { get; set; }
        public IList<BalancesSummary>? BalancesSummary { get; set; }

        //public double TotalAvailableEur { get; set; }
        //public double PrevTotAvailableEur { get; set; }
        //public double TotSalaryEur { get; set; }
        //public double PrevTotSalaryEur { get; set; }
        //public double salaryRefPeriod { get; set; }
    }
}
