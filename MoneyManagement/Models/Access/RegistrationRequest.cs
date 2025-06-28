using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Models.Access;
public class RegistrationRequest
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public string RepeatPassword { get; set; } = null!;
}