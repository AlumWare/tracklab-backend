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
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Order.Infrastructure.Persistence.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(
        connectionString ?? throw new InvalidOperationException("Missing connection string.")
    ));

// Repository registrations
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Resource Context
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Order Context
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Tracking Context
builder.Services.AddScoped<IContainerRepository, ContainerRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<ITrackingEventRepository, TrackingEventRepository>();

// Query services
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
builder.Services.AddScoped<IEmployeeQueryService, EmployeeQueryService>();
builder.Services.AddScoped<IWarehouseQueryService, WarehouseQueryService>();
builder.Services.AddScoped<IPositionQueryService, PositionQueryService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();

// Command services
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IEmployeeCommandService, EmployeeCommandService>();
builder.Services.AddScoped<IWarehouseCommandService, WarehouseCommandService>();
builder.Services.AddScoped<IPositionCommandService, PositionCommandService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();

// Order Command services
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();

// Tracking Command services
builder.Services.AddScoped<IContainerCommandService, ContainerCommandService>();
builder.Services.AddScoped<IRouteCommandService, RouteCommandService>();
builder.Services.AddScoped<ITrackingEventCommandService, TrackingEventCommandService>();

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

app.Run();
