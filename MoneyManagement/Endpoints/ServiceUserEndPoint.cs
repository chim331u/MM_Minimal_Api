using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class ServiceUserEndpoint
{
    public static IEndpointRouteBuilder MapServiceUserEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        
        app.MapGet("/GetServiceUserList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveServiceUserList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetServiceUser/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetServiceUser(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddServiceUser", async (ServiceUser item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ServiceUser is null");
            }
            var result = await service.AddServiceUser(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateServiceUser", async (ServiceUser item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ServiceUser is null");
            }
            var result = await service.UpdateServiceUser(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteServiceUser", async (ServiceUser item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ServiceUser is null");
            }
            var result = await service.DeleteServiceUser(item);
            return result != null ? Results.Ok() : Results.NotFound();
        });
        
        
        return app;
    }
    
}