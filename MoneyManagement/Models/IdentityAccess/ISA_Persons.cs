using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.IdentityAccess
{
    public class ISA_Persons : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        //public ICollection<ISA_Accounts>? ISA_Accounts { get; set; }
    }
}
