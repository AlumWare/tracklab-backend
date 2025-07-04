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
using Alumware.Tracklab.API.Tracking.Domain.Model.Entities;

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
        builder.Entity<Vehicle>().OwnsOne(v => v.Location, loc =>
        {
            loc.Property(p => p.Latitude).HasColumnName("location_latitude");
            loc.Property(p => p.Longitude).HasColumnName("location_longitude");
            loc.WithOwner().HasForeignKey("Id");
        });

        // === WAREHOUSE ===
        builder.Entity<Warehouse>().ToTable("warehouses");
        builder.Entity<Warehouse>().HasKey(w => w.Id);
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
        builder.Entity<Position>().Property(p => p.Name).HasColumnName("name");

        // === PRODUCT ===
        builder.Entity<Product>().ToTable("products");
        builder.Entity<Product>().HasKey(p => p.Id);
        builder.Entity<Product>().OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("price_amount");
            price.Property(p => p.Currency).HasColumnName("price_currency");
            price.WithOwner().HasForeignKey("Id");
        });

        // === ORDER ===
        builder.Entity<Order>().ToTable("orders");
        builder.Entity<Order>().HasKey(o => o.OrderId);
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
        // Configurar relación User-Tenant
        builder.Entity<User>()
            .HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
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

        // === RELACIONES UNO A MUCHOS ===
        // Tenant → Vehicles
        builder.Entity<Vehicle>()
            .HasOne(v => v.Tenant)
            .WithMany(t => t.Vehicles)
            .HasForeignKey(v => v.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tenant → Warehouses
        builder.Entity<Warehouse>()
            .HasOne(w => w.Tenant)
            .WithMany(t => t.Warehouses)
            .HasForeignKey(w => w.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tenant → Products
        builder.Entity<Product>()
            .HasOne(p => p.Tenant)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tenant → Employees
        builder.Entity<Employee>()
            .HasOne(e => e.Tenant)
            .WithMany(t => t.Employees)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tenant → Positions
        builder.Entity<Position>()
            .HasOne(p => p.Tenant)
            .WithMany(t => t.Positions)
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Position → Employees
        builder.Entity<Employee>()
            .HasOne(e => e.Position)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product → OrderItems
        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey("product_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Order → OrderItems
        builder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle → Routes
        builder.Entity<RouteAggregate>()
            .HasOne(r => r.Vehicle)
            .WithMany()
            .HasForeignKey("vehicle_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Warehouse → RouteItems
        builder.Entity<RouteItem>()
            .HasOne(ri => ri.Warehouse)
            .WithMany()
            .HasForeignKey("warehouse_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Route → RouteItems
        builder.Entity<RouteItem>()
            .HasOne(ri => ri.Route)
            .WithMany(r => r.RouteItems)
            .HasForeignKey(ri => ri.RouteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order → Containers
        builder.Entity<Container>()
            .HasOne(c => c.Order)
            .WithMany()
            .HasForeignKey("order_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Warehouse → Containers
        builder.Entity<Container>()
            .HasOne(c => c.Warehouse)
            .WithMany()
            .HasForeignKey("warehouse_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Container → TrackingEvents
        builder.Entity<TrackingEvent>()
            .HasOne(te => te.Container)
            .WithMany(c => c.TrackingEvents)
            .HasForeignKey(te => te.ContainerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Warehouse → TrackingEvents
        builder.Entity<TrackingEvent>()
            .HasOne(te => te.Warehouse)
            .WithMany()
            .HasForeignKey("warehouse_id")
            .OnDelete(DeleteBehavior.Restrict);

        // === RELACIÓN MUCHOS A MUCHOS: Order ↔ Route ===
        builder.Entity<Order>()
            .HasMany(o => o.Routes)
            .WithMany(r => r.Orders)
            .UsingEntity<Dictionary<string, object>>(
                "routes_orders",
                j => j
                    .HasOne<RouteAggregate>()
                    .WithMany()
                    .HasForeignKey("RouteId")
                    .HasConstraintName("FK_routes_orders_route")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Order>()
                    .WithMany()
                    .HasForeignKey("OrderId")
                    .HasConstraintName("FK_routes_orders_order")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("RouteId", "OrderId");
                    j.ToTable("routes_orders");
                }
            );

        builder.Entity<RouteItem>()
            .Property(ri => ri.WarehouseId)
            .HasConversion(
                v => v.Value,
                v => new Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects.WarehouseId(v)
            )
            .HasColumnName("warehouse_id");

        // Tenant → Orders (Customer)
        builder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey("customer_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Tenant → Orders (Logistics)
        builder.Entity<Order>()
            .HasOne(o => o.Logistics)
            .WithMany()
            .HasForeignKey("logistics_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
