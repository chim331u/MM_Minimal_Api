using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.IdentityAccess
{
    public class ISA_Applications : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string WebUrl { get; set; }

        //public ISA_Accounts ISA_Accounts { get; set; }
        public ICollection<ISA_Accounts> ISA_Accounts { get; set; }
        public ICollection<ISA_Tags> ISA_Tags { get; set; }
    }
}
