using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using MoneyManagement.AppContext;
using MoneyManagement.Contract;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Access;
using MoneyManagement.Models.Identity;

namespace MoneyManagement.Services;

public class IdentityService : IIdentityService
{
    private readonly ApplicationContext _context; // Database context
    private readonly ILogger<IdentityService> _logger; // Logger for logging information and errors
    private readonly IUtilityService _utilityServices; // Utility services for encryption and other utilities
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;

    public IdentityService(
        ApplicationContext context,
        ILogger<IdentityService> logger,
        IUtilityService utilityServices,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        _utilityServices = utilityServices;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }


    public async Task<AuthResponse> Login(AuthRequest model)
    {
        if (model == null)
        {
            _logger.LogError("Login model is null");
            return null;
        }

        var managedUser = await _userManager.FindByEmailAsync(model.Email);

        if (managedUser == null)
        {
            _logger.LogWarning($"User with email {model.Email} not found");
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, model.Password);

        if (!isPasswordValid)
        {
            _logger.LogWarning($"Invalid password for user with email {model.Email}");
            return null;
        }
        

        var userInDb = _context.Users.FirstOrDefault(u => u.Email == model.Email);
       
        if (userInDb is null)
        {
            _logger.LogWarning($"User with email {model.Email} not found in database");
            return null;
        }

        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        };
    }
}