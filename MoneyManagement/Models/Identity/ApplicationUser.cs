using Microsoft.AspNetCore.Identity;

namespace MoneyManagement.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}