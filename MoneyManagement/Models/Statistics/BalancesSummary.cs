namespace MoneyManagement.Models.Statistics
{
    public class BalancesSummary
    {
        public string? AccountName { get; set; }
        //public AccountMasterData Account { get; set; }

        public DateTime BalanceDate { get; set; }


        public double BalanceAmount { get; set; }
        public string? Currency { get; set; }

        public double DifferenceAmount { get; set; }

        //public double TotAmount { get; set; }
        //public double TotDiff { get; set; }

        //public double TotAmountOther { get; set; }
        //public double TotDiffOther { get; set; }

        //public double GranTotalAmountEur { get; set; }
        //public double GranTotalDiffEur { get; set; }

    }
}
