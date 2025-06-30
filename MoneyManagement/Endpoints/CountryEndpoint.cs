using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class CountryEndpoint
{
    public static IEndpointRouteBuilder MapCountryEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetCountryList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveCountryList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapGet("/GetCountry/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetCountry(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPost("/AddCountry", async (Country item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Country is null");
            }
            var result = await service.AddCountry(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateCountry", async (Country item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Country is null");
            }
            var result = await service.UpdateCountry(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        return app;
    }
    
}