using MoneyManagement.Contract;

namespace MoneyManagement.Interfaces;

public interface IIdentityServiceV1
{
    Task<ApiResponse<string>> Signup(SignupModelDto model);
    Task<ApiResponse<TokenModelDto>> Login(LoginModelDto model);
    Task<ApiResponse<string>> RefreshToken(TokenModelDto model);
    Task<ApiResponse<string>> RevokeToken(string username);
}