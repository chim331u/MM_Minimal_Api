namespace MoneyManagement.Models.Transactions
{
    public static class TransactionMlConfig
    {
        public static bool IsDev { get; set; }
        public static string MlPath { get; set; }
        public static string TestFileName { get; set; }
        public static string TrainFileName { get; set; }
        public static string ModelName { get; set; }

    }
}
