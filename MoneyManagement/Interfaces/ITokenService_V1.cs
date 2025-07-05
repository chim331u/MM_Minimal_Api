using System.Security.Claims;

namespace MoneyManagement.Interfaces;

public interface ITokenService_V1
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}