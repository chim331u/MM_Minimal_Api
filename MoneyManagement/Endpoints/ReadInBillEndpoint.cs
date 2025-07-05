using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class ReadInBillEndpoint
{
    public static IEndpointRouteBuilder MapReadInBillEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetReadInBillList", async (IAncillaryService service) =>
        {
            var result = await service.GetActiveReadInBillList();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapGet("/GetActiveReadInBillBySupplierList/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetActiveReadInBillBySupplierList(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapGet("/GetReadInBill/{id}", async (int id, IAncillaryService service) =>
        {
            var result = await service.GetReadInBill(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapPost("/AddReadInBill", async (ReadInBill item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ReadInBill is null");
            }
            var result = await service.AddReadInBill(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapPut("/UpdateReadInBill", async (ReadInBill item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ReadInBill is null");
            }
            var result = await service.UpdateReadInBill(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapPut("/DeleteReadInBill", async (ReadInBill item, IAncillaryService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("ReadInBill is null");
            }
            var result = await service.DeleteReadInBill(item);
            return result != null ? Results.Ok() : Results.NotFound();
        });
        
        
        return app;
    }
    
}