using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class CurrencyEndPoint
{
    public static IEndpointRouteBuilder MapCurrencyEndPoint(this IEndpointRouteBuilder app)
    {

        // Define the endpoints
        app.MapGet("/GetCurrencyList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveCurrencyList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetCurrency/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetCurrency(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddCurrency", async (Currency item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Currency is null");
            }
            var result = await service.AddCurrency(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateCurrency", async (Currency item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Currency is null");
            }
            var result = await service.UpdateCurrency(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteCurrency/{id}", async (Currency item, IAncillaryService service) =>
        {
            var result = await service.DeleteCurrency(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
}