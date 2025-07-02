using MoneyManagement.Interfaces;
using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Endpoints;

public static class AccountEndPoint
{
    public static IEndpointRouteBuilder MapAccountEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetAccountList", async (IBankAccountService service) =>
        {
            var result = await service.GetActiveAccountList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapGet("/GetAccountDetail/{id}", async (int id, IBankAccountService service) =>
        {
            var result = await service.GetAccount(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPost("/AddAccount", async (AccountMasterData? item , IBankAccountService service ) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Account is null");
            }
            
            var result = await service.AddAccount(item);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapPut("/UpdateAccount", async (AccountMasterData? item, IBankAccountService service) =>
        {
            if (item==null)
            {
                return Results.BadRequest("Account is null");
            }
            var result = await service.UpdateAccount(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPut("/DeleteAccount", async (AccountMasterData? item, IBankAccountService service) =>
        {
            if (item==null)
            {
                return Results.BadRequest("Account is null");
            }
            var result = await service.DeleteAccount(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
    
}