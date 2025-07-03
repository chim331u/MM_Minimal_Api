using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;

namespace MoneyManagement.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        //public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name cannot have more then 50 characters")]
        public string Name { get; set; } = string.Empty;
    }
}