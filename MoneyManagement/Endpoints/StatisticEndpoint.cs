using MoneyManagement.Interfaces;
using MoneyManagement.Models.AncillaryData;

namespace MoneyManagement.Endpoints;

public static class StatisticEndpoint
{
    public static IEndpointRouteBuilder MapStatisticEndPoint(this IEndpointRouteBuilder app)
    {
        // Define the endpoints
        app.MapGet("/GetDashboard", async (IStatisticService service) =>
        {
            var result = await service.GetDashboard();
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
        
        app.MapGet("/GetSalaryStatistics/{id}", async (int id, IStatisticService service) =>
        {
            var result = await service.GetSalaryStatistic(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

      
        
        return app;
    }
    
}