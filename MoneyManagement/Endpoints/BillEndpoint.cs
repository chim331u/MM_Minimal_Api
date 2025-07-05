using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.Bill;

namespace MoneyManagement.Endpoints;

public static class BillEndpoint
{
    public static IEndpointRouteBuilder MapBillEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetBillList", async (IBillService service) =>
        {
            var result = await service.GetActiveBillList();

            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetBill/{id}", async (int id, IBillService service) =>
        {
            var result = await service.GetBill(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddBill", async (Bill item, IBillService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Bill is null");
            }
            var result = await service.AddBill(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateBill", async (Bill item, IBillService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Bill is null");
            }
            var result = await service.UpdateBill(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/DeleteBill", async (Bill item, IBillService service) =>
        {
            if (item == null)
            {
                return Results.BadRequest("Bill is null");
            }
            var result = await service.DeleteBill(item);
            return result != null ? Results.Ok() : Results.NotFound();
        });
        
        //upload file
        app.MapPost("UploadBill/{id}", async (IFormFile file, int id, IBillService service, IConfiguration config) =>
        {
            var bill = await service.GetBill(id);

            if (bill == null)
            {
                return Results.BadRequest("Bill does not exist");
            }

            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("Please upload a file");
            }

            //check if folder exists
            if (!Directory.Exists(Path.Combine(config["Folders:BillPath"], bill.Supplier.Name)))
            {
                Directory.CreateDirectory(Path.Combine(config["Folders:BillPath"], bill.Supplier.Name));
            }

            //file path - root from config+supplier name
            var _path = Path.Combine(config["Folders:BillPath"], bill.Supplier.Name, string.Concat(bill.PaidDate.ToString("yyyyMM"), "_", bill.BillNumber, Path.GetExtension(file.FileName)));

            try
            {
                using (var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                bill.FullPathFileName = _path;

                await service.UpdateBill(bill);
            }
            catch (Exception ex)
            {
                return Results.BadRequest("File not saved");
            }

            return Results.Ok(bill);
        });
        
     
        
        return app;
    }
    
}