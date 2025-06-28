using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class Currency : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? CurrencyCodeALF3 { get; set; }

        public string? CurrencyCodeNum3 { get; set; }

    }
}
