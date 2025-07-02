using MoneyManagement.Interfaces;
using MoneyManagement.Models.BankAccount;

namespace MoneyManagement.Endpoints;

public static class BankEndPoint
{
    public static IEndpointRouteBuilder MapBankEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetBankList", async (IBankAccountService service) =>
        {
            var result = await service.GetActiveBankList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapGet("/GetBankDetail/{id}", async (int id, IBankAccountService service) =>
        {
            var result = await service.GetBank(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPost("/AddBank", async (BankMasterData? item , IBankAccountService service ) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Bank is null");
            }
            
            var result = await service.AddBank(item);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapPut("/UpdateBank", async (BankMasterData? item, IBankAccountService service) =>
        {
            if (item==null)
            {
                return Results.BadRequest("Bank is null");
            }
            var result = await service.UpdateBank(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPut("/DeleteBank", async (BankMasterData? item, IBankAccountService service) =>
        {
            if (item==null)
            {
                return Results.BadRequest("Bank is null");
            }
            var result = await service.DeleteBank(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
    
}