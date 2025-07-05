using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class CurrencyRateEndPoint
{
    public static IEndpointRouteBuilder MapCurrencyRateEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoint for getting currency rates

        app.MapGet("/GetCurrencyConversionList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveCurrencyConversionList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetCurrencyConversion/{id}", async (IAncillaryService service, int id) =>
        {
            var result = await service.GetCurrencyConversion(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetCurrencyRate/{currencyALF3}", async (IAncillaryService service, string currencyALF3) =>
        {
            var result = await service.GetCurrencyRate(currencyALF3);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        // Define the endpoint for adding a new currency conversion rate
        app.MapPost("/AddCurrencyConversion", async (IAncillaryService service, CurrencyConversionRate item) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Currency conversion rate is null");
            }
            var result = await service.AddCurrencyConversion(item);
            return result != null ? Results.Ok(result) : Results.BadRequest("Error adding currency conversion rate");
        });
        
        // Define the endpoint for updating an existing currency conversion rate
        app.MapPut("/UpdateCurrencyConversion", async (IAncillaryService service, CurrencyConversionRate item) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Currency conversion rate is null");
            }
            var result = await service.UpdateCurrencyConversion(item);
            return result != null ? Results.Ok(result) : Results.BadRequest("Error updating currency conversion rate");
        });
        
        // Define the endpoint for deleting a currency conversion rate
        app.MapPut("/DeleteCurrencyConversion", async (IAncillaryService service, CurrencyConversionRate item) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Currency conversion rate is null");
            }
            var result = await service.DeleteCurrencyConversion(item);
            return result != null ? Results.Ok(result) : Results.BadRequest("Error deleting currency conversion rate");
        });

        // Define the endpoint for updating all currency rates
        app.MapGet("/UpdateCurrencyRate", async (IAncillaryService service) =>
        {
            var result = await service.UpdateCurrencyRate();

            return result switch
            {
                1 => Results.Ok("Rates already updated"),
                0 => Results.Ok("Currency rate updated"),
                -1 => Results.BadRequest("Rate not updated"),
                -2 => Results.BadRequest("Error in update rate"),
                _ => Results.NotFound()
            };
        });
        
        // Define the endpoint for updating all currency rates
        app.MapGet("/UpdateAllCurrencyRate", async (IAncillaryService service) =>
        {
            var result = await service.UpdateAllCurrencyRate();

            return result switch
            {
                1 => Results.Ok("Rates already updated"),
                0 => Results.Ok("Currency rate updated"),
                -1 => Results.BadRequest("Rate not updated"),
                -2 => Results.BadRequest("Error in update rate"),
                _ => Results.NotFound()
            };
        });
        
        app.MapGet("/ClearUnusedRates", async (IAncillaryService service) =>
        {
            var result = await service.ClearUnusedRates();
            return string.IsNullOrEmpty(result)  ? Results.Ok("Currency conversion cache cleared successfully") : Results.BadRequest("Error clearing currency conversion cache");
        });
        return app;
    }
}