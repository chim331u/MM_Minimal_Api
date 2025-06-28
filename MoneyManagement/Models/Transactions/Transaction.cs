using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.BankAccount;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.Transactions
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime TxnDate { get; set; }
        public double TxnAmount { get; set; }
        public string? Description { get; set; }
        public string? UniqueKey { get; set; } //concat currencyCodeAlf3+rateValue+referringDate(only date, no time)

        public bool IsCatConfirmed { get; set; }
        public string? Area { get; set; } //category, tab

        public AccountMasterData Account { get; set; }

    }
}
