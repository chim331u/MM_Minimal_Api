using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.BankAccount;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.Balance
{
    public class Balance : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public double BalanceValue { get; set; }
        public DateTime DateBalance { get; set; }

        public AccountMasterData? Account { get; set; }
    }
}
