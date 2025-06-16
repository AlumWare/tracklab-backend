using Microsoft.EntityFrameworkCore;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;
using Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;
using TrackLab.IAM.Infrastructure.Configuration;
using TrackLab.Shared.Infrastructure.Documentation.OpenApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Load connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(
        connectionString ?? throw new InvalidOperationException("Missing connection string.")
    ));

// Repository registrations
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();

// Query services
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
builder.Services.AddScoped<IEmployeeQueryService, EmployeeQueryService>();
builder.Services.AddScoped<IWarehouseQueryService, WarehouseQueryService>();
builder.Services.AddScoped<IPositionQueryService, PositionQueryService>();

// Command services
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IEmployeeCommandService, EmployeeCommandService>();
builder.Services.AddScoped<IWarehouseCommandService, WarehouseCommandService>();
builder.Services.AddScoped<IPositionCommandService, PositionCommandService>();

// Add IAM Configuration
builder.Services.AddIamConfiguration(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger with JWT Configuration
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Auto-create DB in dev
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrackLab API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "TrackLab API Documentation";
        c.DefaultModelsExpandDepth(-1); // Hide schemas section by default
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapse all operations by default
    });
}

// Middleware pipeline
app.UseHttpsRedirection();

// Add IAM middleware (authentication + authorization)
app.UseIamConfiguration();

// Map controllers
app.MapControllers();

// Add a welcome endpoint
app.MapGet("/", () => new
{
    message = "¡Bienvenido a TrackLab API!",
    version = "v1.0",
    documentation = "/swagger",
    endpoints = new
    {
        health_public = "/api/v1/health/public",
        health_protected = "/api/v1/health/protected",
        auth_signup = "/api/v1/authentication/sign-up",
        auth_signin = "/api/v1/authentication/sign-in"
    }
});

app.Run();
