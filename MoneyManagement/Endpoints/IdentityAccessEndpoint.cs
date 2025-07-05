using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.IdentityAccess;

namespace MoneyManagement.Endpoints;

public static class IdentityAccessEndpoint
{
    public static IEndpointRouteBuilder MapIdentityAccessEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetIdentityAccessList", async (IIdentityAccessService service) =>
        {
            var result = await service.GetActiveIdentityAccountList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetIdentityAccess/{id}", async (int id, IIdentityAccessService service) =>
        {
            var result = await service.GetIdentityAccount(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddIdentityAccess", async (ISA_Accounts item, IIdentityAccessService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Identity Access is null");
            }
            var result = await service.AddIdentityAccount(item);
            return result != null ? Results.Ok(result) : Results.BadRequest("Failed to add identity access");
        });
        
        app.MapPut("/UpdateIdentityAccess", async (ISA_Accounts item, IIdentityAccessService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Identity Access is null");
            }
            var result = await service.UpdateIdentityAccount(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteIdentityAccess", async (ISA_Accounts item, IIdentityAccessService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Identity Access is null");
            }
            var result = await service.DeleteIdentityAccount(item);
            return result != null ? Results.Ok("Identity Access deleted successfully") : Results.NotFound();
        });
        
        app.MapGet("/GetOldPasswordsList/{id}", async (int id, IIdentityAccessService service) =>
        {
            var result = await service.ListAllOldPasswords(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetCleanPsw/{id}", async (int id, IIdentityAccessService service) =>
        {
            var result = await service.GetCleanPsw(id);
            return !string.IsNullOrEmpty(result) ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/PasswordChange", async (ISA_Accounts item, IIdentityAccessService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Password is null");
            }
            var result = await service.PasswordChange(item);
            return !string.IsNullOrEmpty(result) ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
    
}