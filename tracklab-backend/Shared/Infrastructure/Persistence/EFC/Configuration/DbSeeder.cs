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
        
        var secondTenant = new Tenant(
            "20123456790", 
            "Empresa Test S.A.C.", 
            "EmpresaTest", 
            "Av. Test 456", 
            "Lima", 
            "Perú",
            new PhoneNumber("+51987654321"),
            new Email("info@empresatest.com"),
            "https://empresatest.com"
        );
        
        context.Tenants.AddRange(defaultTenant, secondTenant);

        // Guardar primero los tenants para obtener sus IDs
        await context.SaveChangesAsync();

        // Seed Users para el primer tenant
        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = hashingService.HashPassword("admin123"),
            Email = new Email("admin@tracklab.com"),
            FirstName = "Admin",
            LastName = "User",
            Active = true,
            TenantId = defaultTenant.Id
        };

        var testUser = new User
        {
            Username = "test",
            PasswordHash = hashingService.HashPassword("test123"),
            Email = new Email("test@tracklab.com"),
            FirstName = "Test",
            LastName = "User",
            Active = true,
            TenantId = defaultTenant.Id
        };

        // Seed Users para el segundo tenant
        var hakiUser = new User
        {
            Username = "haki",
            PasswordHash = hashingService.HashPassword("haki123"),
            Email = new Email("usuario@empresa.com"),
            FirstName = "Juan",
            LastName = "Pérez",
            Active = true,
            TenantId = secondTenant.Id
        };

        context.Users.AddRange(adminUser, testUser, hakiUser);

        // Guardar todos los cambios
        await context.SaveChangesAsync();
        
        Console.WriteLine("✅ Base de datos inicializada con datos de prueba");
        Console.WriteLine($"   👤 Usuario admin: admin / admin123 (Tenant {defaultTenant.Id})");
        Console.WriteLine($"   👤 Usuario test: test / test123 (Tenant {defaultTenant.Id})");
        Console.WriteLine($"   👤 Usuario haki: haki / haki123 (Tenant {secondTenant.Id})");
        Console.WriteLine($"   🏢 Tenant 1: {defaultTenant.GetDisplayName()}");
        Console.WriteLine($"   🏢 Tenant 2: {secondTenant.GetDisplayName()}");
    }
} 