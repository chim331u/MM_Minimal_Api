using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class CurrencyConversionRate : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public decimal RateValue { get; set; }
        public string? CurrencyCodeALF3 { get; set; }

        public DateTime ReferringDate { get; set; }

        public string? UniqueKey { get; set; } //concat currencyCodeAlf3+rateValue+referringDate(only date, no time)


    }
}
