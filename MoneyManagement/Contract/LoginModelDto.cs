using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Contract;

public class LoginModelDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}