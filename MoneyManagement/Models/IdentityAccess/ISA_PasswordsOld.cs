using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.IdentityAccess
{
    public class ISA_PasswordsOld : BaseEntity
    {

        public int Id { get; set; }

        public string? Password { get; set; }

        //public ICollection<ISA_Accounts> ISA_Accounts { get; set; }
        public ISA_Accounts? ISA_Account { get; set; }
    }
}
