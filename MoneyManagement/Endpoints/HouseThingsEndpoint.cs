using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.HouseThings;

namespace MoneyManagement.Endpoints;

public static class HouseThingsEndpoint
{
    public static IEndpointRouteBuilder MapHouseThingsEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetHouseThingsList", async (IHouseThingsService service) =>
        {
            var result = await service.GetActiveHouseThingsList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetHouseThingsListByRoom/{id}", async (int id, IHouseThingsService service) =>
        {
            var result = await service.GetActiveHouseThingsListByRoom(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetHistoryHouseThingsList/{id}", async (int id, IHouseThingsService service) =>
        {
            var result = await service.GetHistoryHouseThingsList(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetHouseThings/{id}", async (int id, IHouseThingsService service) =>
        {
            var result = await service.GetHouseThings(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddHouseThings", async (HouseThings item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThings is null");
            }
            var result = await service.AddHouseThings(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/RenewHouseThings", async (HouseThings item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThings is null");
            }
            var result = await service.RenewHouseThings(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateHouseThings", async (HouseThings item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThings is null");
            }
            var result = await service.UpdateHouseThings(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteHouseThings", async (HouseThings item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThings is null");
            }
            var result = await service.DeleteHouseThings(item);
            return result != null ? Results.Ok() : Results.NotFound();
        });
        
        
        return app;
    }
    
}