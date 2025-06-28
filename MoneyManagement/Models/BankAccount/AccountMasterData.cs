using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.BankAccount
{
    public class AccountMasterData : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Conto { get; set; } //NEW 
        public string? Description { get; set; }
        public string? Iban { get; set; }
        public string? Bic { get; set; }
        public string? AccountType { get; set; }

        //Saldo
        //Data Saldo

        public Currency? Currency { get; set; }
        public BankMasterData? BankMasterData { get; set; }

        //Cards
        //public ICollection<CardMasterData>? CardMasterData { get; set; }
        //public ICollection<Balance>? Balances { get; set; }


    }
}
