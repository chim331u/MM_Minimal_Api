using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Contract
{
    public class Account_DTO
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Conto { get; set; } //NEW 
        public string? Description { get; set; }
        public string? Iban { get; set; }
        public string? Bic { get; set; }
        public string? AccountType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Note { get; set; }

        public int CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public Currency Currency { get; set; }

        public int BankId{ get; set; }
        public string? BankName{ get; set; }
    }
}
