using Microsoft.EntityFrameworkCore;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.IAM.Application.Internal.OutboundServices;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IHashingService hashingService)
    {
        Console.WriteLine("🌱 Iniciando seeding de la base de datos...");

        // Seed Tenants
        var defaultTenant = new Tenant(
            "20123456789", 
            "TrackLab S.A.C.", 
            "TrackLab", 
            "Av. Tecnología 123", 
            "Lima", 
            "Perú",
            new PhoneNumber("+51912345678"),
            new Email("info@tracklab.com"),
            "https://tracklab.com"
        );
        context.Tenants.Add(defaultTenant);

        // Guardar primero el tenant para obtener su ID
        await context.SaveChangesAsync();

        // Hash the passwords correctly using the hashing service
        var adminPasswordHash = hashingService.HashPassword("Admin123!");
        var userPasswordHash = hashingService.HashPassword("User123!");

        // Seed Users
        var adminUser = new User(
            "admin",
            adminPasswordHash,
            new Email("admin@tracklab.com"),
            "Admin",
            "TrackLab",
            new TenantId(defaultTenant.Id)
        );
        adminUser.AddRole(Role.Admin);

        var regularUser = new User(
            "user",
            userPasswordHash,
            new Email("user@tracklab.com"),
            "Usuario",
            "Estándar",
            new TenantId(defaultTenant.Id)
        );
        regularUser.AddRole(Role.Operator);

        context.Users.AddRange(adminUser, regularUser);

        // Guardar todos los cambios
        await context.SaveChangesAsync();
        
        Console.WriteLine("✅ Base de datos inicializada con datos de prueba");
        Console.WriteLine($"   👤 Usuario admin: admin / Admin123!");
        Console.WriteLine($"   👤 Usuario operador: user / User123!");
        Console.WriteLine($"   🏢 Tenant: {defaultTenant.GetDisplayName()}");
    }
} 