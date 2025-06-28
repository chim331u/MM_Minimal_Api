using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.IdentityAccess
{
    public class ISA_Accounts : BaseEntity
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? WebUrl { get; set; }


        public ServiceUser? ISA_User { get; set; }
        public string? ISA_Tag { get; set; }

        public ICollection<ISA_PasswordsOld>? ISA_PasswordsOld { get; set; }

    }
}

