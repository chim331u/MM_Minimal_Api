using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class Country : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Country name cannot have more then 50 characters.")]
        public string? Name { get; set; }
        [MaxLength(200, ErrorMessage = "Country Description cannot have more then 200 characters.")]
        public string? Description { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Country code Alphanumeric must be lenght 3 characters")]
        public string? CountryCodeALF3 { get; set; }
        [MaxLength(3, ErrorMessage = "Country code Numeric must be lenght 3 numbers")]
        public string? CountryCodeNum3 { get; set; }

        //public ICollection<BankMasterData?> BankMasterData { get; set; }

        //TODO Currency(RIF)

    }
}
