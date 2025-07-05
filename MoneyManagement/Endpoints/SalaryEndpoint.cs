using MoneyManagement.Interfaces;
using MoneyManagement.Models.Salary;

namespace MoneyManagement.Endpoints;

public static class SalaryEndpoint
{
    public static IEndpointRouteBuilder MapSalaryEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints

        app.MapGet("/GetSalaryList", async (ISalaryService salaryService) =>
        {
            var result = await salaryService.GetActiveSalaryList();
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetSalary/{salaryId}", async (int salaryId, ISalaryService salaryService) =>
        {
            var result = await salaryService.GetSalary(salaryId);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPut("/UpdateSalary", async (Salary item, ISalaryService salaryService) =>
        {
            var result = await salaryService.UpdateSalary(item);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/AddSalary", async (Salary item, ISalaryService salaryService) =>
        {
            var result = await salaryService.AddSalary(item);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapPost("/DeleteSalary", async (Salary salary, ISalaryService salaryService) =>
        {
            var result = await salaryService.DeleteSalary(salary);
            return result is not null ? Results.Ok() : Results.NotFound();
        });
        
        return app;
    }
    
}