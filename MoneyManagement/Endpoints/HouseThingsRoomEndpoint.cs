using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.HouseThings;

namespace MoneyManagement.Endpoints;

public static class HouseThingsRoomEndpoint
{
    public static IEndpointRouteBuilder MapHouseThingsRoomEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetHouseThingsRoomList", async (IHouseThingsService service) =>
        {
            var result = await service.GetActiveHouseThingsRoomsList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetHouseThingsRoom/{id}", async (int id, IHouseThingsService service) =>
        {
            var result = await service.GetHouseThingsRooms(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddHouseThingsRoom", async (HouseThingsRooms item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThingsRoom is null");
            }

            var result = await service.AddHouseThingsRooms(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateHouseThingsRoom", async (HouseThingsRooms item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThingsRoom is null");
            }

            var result = await service.UpdateHouseThingsRooms(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteHouseThingsRoom", async (HouseThingsRooms item, IHouseThingsService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("HouseThingsRoom is null");
            }

            var result = await service.DeleteHouseThingsRooms(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        
        return app;
    }
    
}