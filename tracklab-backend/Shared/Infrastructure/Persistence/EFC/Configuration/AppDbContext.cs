using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using EFCore.NamingConventions;

using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Entities;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using TrackLab.IAM.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Product> Products => Set<Product>();
    
    // Order Context
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    
    // IAM Context
    public DbSet<User> Users => Set<User>();
    public DbSet<Tenant> Tenants => Set<Tenant>();

    // Tracking Context
    public DbSet<RouteAggregate> Routes => Set<RouteAggregate>();
    public DbSet<Container> Containers => Set<Container>();
    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseSnakeCaseNamingConvention();
        // === EMPLOYEE ===
        builder.Entity<Employee>().ToTable("employees");
        builder.Entity<Employee>().HasKey(e => e.Id);
        builder.Entity<Employee>().OwnsOne(e => e.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Employee>().OwnsOne(e => e.Dni, d =>
        {
            d.Property(p => p.Value).HasColumnName("dni");
            d.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Employee>().OwnsOne(e => e.Email, e =>
        {
            e.Property(p => p.Value).HasColumnName("email");
            e.WithOwner().HasForeignKey("Id");
        });

        // === VEHICLE ===
        builder.Entity<Vehicle>().ToTable("vehicles");
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().OwnsOne(v => v.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Vehicle>().OwnsOne(v => v.Location, loc =>
        {
            loc.Property(p => p.Latitude).HasColumnName("location_latitude");
            loc.Property(p => p.Longitude).HasColumnName("location_longitude");
            loc.WithOwner().HasForeignKey("Id");
        });

        // === WAREHOUSE ===
        builder.Entity<Warehouse>().ToTable("warehouses");
        builder.Entity<Warehouse>().HasKey(w => w.Id);
        builder.Entity<Warehouse>().OwnsOne(w => w.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Warehouse>().OwnsOne(w => w.Coordinates, c =>
        {
            c.Property(p => p.Latitude).HasColumnName("latitude");
            c.Property(p => p.Longitude).HasColumnName("longitude");
            c.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Warehouse>().OwnsOne(w => w.Address, a =>
        {
            a.Property(p => p.Value).HasColumnName("address");
            a.WithOwner().HasForeignKey("Id");
        });

        // === POSITION ===
        builder.Entity<Position>().ToTable("positions");
        builder.Entity<Position>().HasKey(p => p.Id);
        builder.Entity<Position>().OwnsOne(p => p.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });

        // === PRODUCT ===
        builder.Entity<Product>().ToTable("products");
        builder.Entity<Product>().HasKey(p => p.Id);
        builder.Entity<Product>().OwnsOne(p => p.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Product>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("price_amount");
            price.Property(p => p.Currency).HasColumnName("price_currency");
            price.WithOwner().HasForeignKey("Id");
        });

        // === ORDER ===
        builder.Entity<Order>().ToTable("orders");
        builder.Entity<Order>().HasKey(o => o.OrderId);
        builder.Entity<Order>().OwnsOne(o => o.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("customer_id");
            t.WithOwner().HasForeignKey("OrderId");
        });
        builder.Entity<Order>().OwnsOne(o => o.LogisticsId, l =>
        {
            l.Property(p => p.Value).HasColumnName("logistics_id");
            l.WithOwner().HasForeignKey("OrderId");
        });
        builder.Entity<Order>().Property(o => o.OrderDate).HasColumnName("order_date");
        builder.Entity<Order>().Property(o => o.Status).HasColumnName("status");

        // === ORDER ITEM ===
        builder.Entity<OrderItem>().ToTable("order_items");
        builder.Entity<OrderItem>().HasKey(oi => oi.Id);
        builder.Entity<OrderItem>().OwnsOne(oi => oi.ProductId, p =>
        {
            p.Property(pid => pid.Value).HasColumnName("product_id");
            p.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<OrderItem>().OwnsOne(oi => oi.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("price_amount");
            price.Property(p => p.Currency).HasColumnName("price_currency");
            price.WithOwner().HasForeignKey("Id");
        });

        // === IAM CONTEXT ===
        
        // === USER ===
        builder.Entity<User>().ToTable("users");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().OwnsOne(u => u.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
            t.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<User>().OwnsOne(u => u.Email, e =>
        {
            e.Property(p => p.Value).HasColumnName("email");
            e.WithOwner().HasForeignKey("Id");
        });
        
        // Configurar roles como una propiedad string separado por comas
        builder.Entity<User>()
            .Property(u => u.RolesInternal)
            .HasConversion(
                roles => string.Join(",", roles.Select(r => r.Name)),
                rolesString => rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => TrackLab.IAM.Domain.Model.ValueObjects.Role.FromName(name))
                    .ToList()
            )
            .HasColumnName("roles");
            
        // Ignorar completamente la propiedad Roles de solo lectura
        builder.Entity<User>()
            .Ignore(u => u.Roles);
            
        // Asegurar que no se creen tablas adicionales para roles
        builder.Ignore<TrackLab.IAM.Domain.Model.ValueObjects.Role>();

        // === TENANT ===
        builder.Entity<Tenant>().ToTable("tenants");
        builder.Entity<Tenant>().HasKey(t => t.Id);
        builder.Entity<Tenant>().OwnsOne(t => t.Email, e =>
        {
            e.Property(p => p.Value).HasColumnName("email");
            e.WithOwner().HasForeignKey("Id");
        });
        builder.Entity<Tenant>().OwnsOne(t => t.PhoneNumber, p =>
        {
            p.Property(p => p.Value).HasColumnName("phone_number");
            p.WithOwner().HasForeignKey("Id");
        });

        // === TRACKING ===
        builder.Entity<RouteAggregate>().ToTable("routes");
        builder.Entity<RouteAggregate>().HasKey(r => r.RouteId);
        builder.Entity<RouteAggregate>().OwnsOne(r => r.VehicleId, v =>
        {
            v.Property(p => p.Value).HasColumnName("vehicle_id");
            v.WithOwner().HasForeignKey("RouteId");
        });
        builder.Entity<RouteAggregate>().OwnsMany(r => r.RouteItems, ri =>
        {
            ri.WithOwner().HasForeignKey("RouteId");
            ri.Property(p => p.WarehouseId).HasConversion(
                v => v.Value,
                v => new Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects.WarehouseId(v)
            ).HasColumnName("warehouse_id");
            ri.Property(p => p.IsCompleted).HasColumnName("is_completed");
            ri.Property(p => p.CompletedAt).HasColumnName("completed_at");
            ri.HasKey("Id");
        });
        builder.Entity<RouteAggregate>().OwnsMany(r => r.Orders, o =>
        {
            o.WithOwner().HasForeignKey("RouteId");
            o.Property(p => p.Value).HasColumnName("order_id");
            o.HasKey("Id");
        });

        builder.Entity<Container>().ToTable("containers");
        builder.Entity<Container>().HasKey(c => c.ContainerId);
        builder.Entity<Container>().OwnsOne(c => c.OrderId, o =>
        {
            o.Property(p => p.Value).HasColumnName("order_id");
            o.WithOwner().HasForeignKey("ContainerId");
        });
        builder.Entity<Container>().OwnsOne(c => c.WarehouseId, w =>
        {
            w.Property(p => p.Value).HasColumnName("warehouse_id");
            w.WithOwner().HasForeignKey("ContainerId");
        });
        builder.Entity<Container>().OwnsMany(c => c.ShipItems, si =>
        {
            si.WithOwner().HasForeignKey("ContainerId");
            si.Property(p => p.ProductId).HasColumnName("product_id");
            si.Property(p => p.Quantity).HasColumnName("quantity");
            si.HasKey("Id");
        });

        builder.Entity<TrackingEvent>().ToTable("tracking_events");
        builder.Entity<TrackingEvent>().HasKey(e => e.EventId);
        builder.Entity<TrackingEvent>().Property(e => e.ContainerId).HasColumnName("container_id");
        builder.Entity<TrackingEvent>().OwnsOne(e => e.WarehouseId, w =>
        {
            w.Property(p => p.Value).HasColumnName("warehouse_id");
            w.WithOwner().HasForeignKey("EventId");
        });
        builder.Entity<TrackingEvent>().Property(e => e.Type).HasColumnName("type");
        builder.Entity<TrackingEvent>().Property(e => e.EventTime).HasColumnName("event_time");
    }
}
