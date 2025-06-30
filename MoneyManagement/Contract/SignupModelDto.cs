using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Contract;

public class SignupModelDto
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Password { get; set; } = string.Empty;
}