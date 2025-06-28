using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class Country : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? CountryCodeALF3 { get; set; }
        public string? CountryCodeNum3 { get; set; }

        //public ICollection<BankMasterData?> BankMasterData { get; set; }

        //TODO Currency(RIF)

    }
}
