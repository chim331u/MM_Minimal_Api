using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Contract;

public class TokenModelDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}