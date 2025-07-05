using Microsoft.AspNetCore.Mvc;
using MoneyManagement.Contract;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.Access;
using Newtonsoft.Json;

namespace MoneyManagement.Endpoints;

public static class AccessEndPoint
{
    public static IEndpointRouteBuilder MapAccessEndPoint(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/login", async ([FromBody]AuthRequest authRequest, IAccessService service ) =>
        // {
        //     var result = await service.Login(authRequest);
        //     return result != null ? Results.Ok(result) : Results.NotFound();
        // });
        
        app.MapPost("/login", async (LoginModelDto authRequest, IIdentityService identityService) =>
        {
            var result = await identityService.Login(authRequest);
            return result != null ? Results.Ok(result) : Results.NotFound();

        });

        return app;
    }
    
}