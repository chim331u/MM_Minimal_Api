using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.BankAccount
{
    public class BankMasterData : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bank name is required")]
        public string Name { get; set; } //bank name

        [StringLength(50, ErrorMessage = "Description too long (50 character limit).")]
        public string? Description { get; set; }

        public string? WebUrl { get; set; }
        //address
        public string? Address { get; set; }
        public string? City { get; set; }
        //reference
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public string? ReferenceName { get; set; }

        public Country? Country { get; set; }

        //Accounts
        //public ICollection<AccountMasterData> AccountMasterData { get; set; }

    }
}
