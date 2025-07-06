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
using TrackLab.IAM.Application.Internal.OutboundServices;
using TrackLab.Shared.Infrastructure.Documentation.OpenApi.Configuration;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Order.Infrastructure.Persistence.Repositories;
using Alumware.Tracklab.API.Order.Infrastructure.Persistence.EFC.Repositories;
using Alumware.Tracklab.API.Order.Application.Internal.QueryServices;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Tracking.Application.Internal.QueryServices;
using Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;
using Alumware.Tracklab.API.IAM.Infrastructure.Pipeline.Middleware.Components;
using TrackingEventRepositoryTracking = Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories.TrackingEventRepository;
using TrackingEventRepositoryOrder = Alumware.Tracklab.API.Order.Infrastructure.Persistence.EFC.Repositories.TrackingEventRepository;
using ITrackingEventRepositoryTracking = Alumware.Tracklab.API.Tracking.Domain.Repositories.ITrackingEventRepository;
using ITrackingEventRepositoryOrder = Alumware.Tracklab.API.Order.Domain.Repositories.ITrackingEventRepository;
using ITrackingEventCommandServiceTracking = Alumware.Tracklab.API.Tracking.Domain.Services.ITrackingEventCommandService;
using ITrackingEventCommandServiceOrder = Alumware.Tracklab.API.Order.Domain.Services.ITrackingEventCommandService;
using TrackingEventCommandServiceTracking = Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices.TrackingEventCommandService;
using TrackingEventCommandServiceOrder = Alumware.Tracklab.API.Order.Application.Internal.CommandServices.TrackingEventCommandService;

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
builder.Services.AddScoped<ITrackingEventRepositoryOrder, TrackingEventRepositoryOrder>();

// Tracking Context
builder.Services.AddScoped<IContainerRepository, ContainerRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<ITrackingEventRepositoryTracking, TrackingEventRepositoryTracking>();

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
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<ITrackingEventCommandServiceOrder, TrackingEventCommandServiceOrder>();

// Tracking Command services
builder.Services.AddScoped<IContainerCommandService, ContainerCommandService>();
builder.Services.AddScoped<IRouteCommandService, RouteCommandService>();
builder.Services.AddScoped<ITrackingEventCommandServiceTracking, TrackingEventCommandServiceTracking>();

// Tracking Query services
builder.Services.AddScoped<IContainerQueryService, ContainerQueryService>();
builder.Services.AddScoped<IRouteQueryService, RouteQueryService>();
builder.Services.AddScoped<ITrackingEventQueryService, TrackingEventQueryService>();

// Add IAM Configuration
builder.Services.AddIamConfiguration(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger with JWT Configuration
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Auto-recreate DB and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hashingService = scope.ServiceProvider.GetRequiredService<IHashingService>();
    
    //Delete and recreate the database (for development)
    Console.WriteLine("Deleting existing database...");
    await dbContext.Database.EnsureDeletedAsync();
    Console.WriteLine("Creating new database...");
    await dbContext.Database.EnsureCreatedAsync();

    
    // Seed the database
    await DbSeeder.SeedAsync(dbContext, hashingService);
}

// Swagger config ( production and development )
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrackLab API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "TrackLab API Documentation";
    c.DefaultModelsExpandDepth(-1); 
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); 
    
    // Production config
    if (!app.Environment.IsDevelopment())
    {
        c.DisplayRequestDuration();
        c.ShowExtensions();
    }
});

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

// Add IAM middleware (authentication + authorization)
app.UseIamConfiguration();

// Map controllers
app.MapControllers();

app.Run();
