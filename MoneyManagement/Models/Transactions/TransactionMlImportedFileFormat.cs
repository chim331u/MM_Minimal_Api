using Microsoft.ML.Data;

namespace MoneyManagement.Models.Transactions
{
    public class TransactionMlImportedFileFormat
    {
        [LoadColumn(0)]
        public int Id { get; set; }

        [LoadColumn(1)]
        public string Area { get; set; }

        [LoadColumn(2)]
        public string Name { get; set; }

        [LoadColumn(3)]
        public string Amount { get; set; }

    }
}
