using MoneyManagement.Interfaces;
using MoneyManagement.Models.Balance;

namespace MoneyManagement.Endpoints;

public static class BalanceEndpoint
{
    public static IEndpointRouteBuilder MapBalanceEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints

        app.MapGet("/GetBalanceList", async (IBalanceService service) =>
        {
            var result = await service.GetActiveBalanceList();
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapGet("/GetBalance/{id}", async (int id, IBalanceService service) =>
        {
            var result = await service.GetBalance(id);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapPut("/AddBalance", async (Balance item, IBalanceService service) =>
        {
            var result = await service.AddBalance(item);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapPost("/UpdateBalance", async (Balance item, IBalanceService service) =>
        {
            var result = await service.UpdateBalance(item);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        app.MapPost("/DeleteBalance", async (Balance item, IBalanceService service) =>
        {
            var result = await service.DeleteBalance(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
    
}