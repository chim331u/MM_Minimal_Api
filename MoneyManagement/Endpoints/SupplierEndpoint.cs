using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class SupplierEndpoint
{
    public static IEndpointRouteBuilder MapSupplierEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetSupplierList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveSupplierList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetSupplier/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetSupplier(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddSupplier", async (Supplier item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Supplier is null");
            }
            var result = await service.AddSupplier(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateSupplier", async (Supplier item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Supplier is null");
            }
            var result = await service.UpdateSupplier(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteSupplier", async (Supplier item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Supplier is null");
            }
            var result = await service.DeleteSupplier(item);
            return result != null ? Results.Ok() : Results.NotFound();
        });
        
        return app;
    }
    
}