using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoneyManagement.AppContext;
using MoneyManagement.Endpoints;
using MoneyManagement.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "MM Minimal API", Version = "v1", Description = "Money Management minimal API" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader()
            //.AllowCredentials()
            ;
    });
});

// Configure logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

//Appy migrations on app start
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    db.Database.Migrate();
}

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); //it is new line
app.UseAuthorization();
app.UseHttpsRedirection();


//TODO add authentication to all endpoints

app.MapGroup("/api/v1/")
    .WithTags(" Access endpoints")
    .MapAccessEndPoint_V1();

app.MapGroup("/api/access/")
    .WithTags(" Access endpoints")
    .MapAccessEndPoint();

app.MapGroup("/api/Country/")
    .WithTags(" Country endpoints")
    .RequireAuthorization()
    .MapCountryEndPoint();

app.MapGroup("/api/Currency/")
    .WithTags(" Currency endpoints")
    //.RequireAuthorization()
    .MapCurrencyEndPoint();

app.MapGroup("/api/Account/")
    .WithTags(" Account endpoints")
    //.RequireAuthorization()
    .MapAccountEndPoint();

app.MapGroup("/api/Bank/")
    .WithTags(" Bank endpoints")
    //.RequireAuthorization()
    .MapBankEndPoint();

app.MapGroup("/api/Balance/")
    .WithTags(" Balance endpoints")
    //.RequireAuthorization()
    .MapBalanceEndPoint();

app.MapGroup("/api/Salary/")
    .WithTags(" Salary endpoints")
    //.RequireAuthorization()
    .MapSalaryEndPoint();

app.MapGroup("/api/CurrencyConversion/")
    .WithTags(" CurrencyConvRate endpoints")
    //.RequireAuthorization()
    .MapCurrencyRateEndPoint();

app.MapGroup("/api/ReadInBill/")
    .WithTags(" Read Bill endpoint endpoints")
    //.RequireAuthorization()
    .MapReadInBillEndPoint();

app.MapGroup("/api/Supplier/")
    .WithTags(" Supplier endpoints")
    //.RequireAuthorization()
    .MapSupplierEndPoint();

app.MapGroup("/api/ServiceUser/")
    .WithTags(" Service User endpoints")
    //.RequireAuthorization()
    .MapServiceUserEndPoint();

app.MapGroup("/api/Bill/")
    .WithTags(" Bill endpoints")
    //.RequireAuthorization()
    .MapBillEndPoint();

app.MapGroup("/api/HouseThings/")
    .WithTags(" House Things endpoints")
    //.RequireAuthorization()
    .MapHouseThingsEndPoint();

app.MapGroup("/api/HouseThingsRooms/")
    .WithTags(" House Things Room endpoints")
    //.RequireAuthorization()
    .MapHouseThingsRoomEndPoint();

app.MapGroup("/api/Transaction/")
    .WithTags(" Transaction endpoints")
    //.RequireAuthorization()
    .MapTransactionEndPoint();

app.MapGroup("/api/Statistic/")
    .WithTags(" Statistics endpoints")
    //.RequireAuthorization()
    .MapStatisticEndPoint();

app.MapGroup("/api/IdentiyAccess/")
    .WithTags(" Identity Access endpoints")
    //.RequireAuthorization()
    .MapIdentityAccessEndPoint();

app.Run();
