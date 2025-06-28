using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Contract
{
    public class Balance_DTO
    {
        [Key]
        public int Id { get; set; }
        public double BalanceValue { get; set; }
        public DateTime DateBalance { get; set; }
        public DateTime CreatedDate {  get; set; }
        public string? Note { get; set; }
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? CurrencyName { get; set; }
    }
}
