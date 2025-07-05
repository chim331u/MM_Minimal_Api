using MoneyManagement.Contract;
using MoneyManagement.Models.Access;

namespace MoneyManagement.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse> Login(AuthRequest model);
    
 }