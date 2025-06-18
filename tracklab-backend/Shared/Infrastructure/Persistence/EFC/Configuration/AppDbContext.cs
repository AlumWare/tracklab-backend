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
    }
}
