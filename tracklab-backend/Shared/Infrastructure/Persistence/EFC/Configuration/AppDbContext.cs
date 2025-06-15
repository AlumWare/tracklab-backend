using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using EFCore.NamingConventions;

using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Position> Positions => Set<Position>();

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
        builder.Entity<Employee>().OwnsOne(e => e.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
        });
        builder.Entity<Employee>().OwnsOne(e => e.Dni, d =>
        {
            d.Property(p => p.Value).HasColumnName("dni");
        });
        builder.Entity<Employee>().OwnsOne(e => e.Email, e =>
        {
            e.Property(p => p.Value).HasColumnName("email");
        });

        // === VEHICLE ===
        builder.Entity<Vehicle>().ToTable("vehicles");
        builder.Entity<Vehicle>().OwnsOne(v => v.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
        });
        builder.Entity<Vehicle>().OwnsOne(v => v.Location, loc =>
        {
            loc.Property(p => p.Latitude).HasColumnName("location_latitude");
            loc.Property(p => p.Longitude).HasColumnName("location_longitude");
        });

        // === WAREHOUSE ===
        builder.Entity<Warehouse>().ToTable("warehouses");
        builder.Entity<Warehouse>().OwnsOne(w => w.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
        });
        builder.Entity<Warehouse>().OwnsOne(w => w.Coordinates, c =>
        {
            c.Property(p => p.Latitude).HasColumnName("latitude");
            c.Property(p => p.Longitude).HasColumnName("longitude");
        });
        builder.Entity<Warehouse>().OwnsOne(w => w.Address, a =>
        {
            a.Property(p => p.Value).HasColumnName("address");
        });

        // === POSITION ===
        builder.Entity<Position>().ToTable("positions");
        builder.Entity<Position>().OwnsOne(p => p.TenantId, t =>
        {
            t.Property(p => p.Value).HasColumnName("tenant_id");
        });
    }
}
