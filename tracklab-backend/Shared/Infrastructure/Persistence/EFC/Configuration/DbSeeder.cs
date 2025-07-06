using Microsoft.EntityFrameworkCore;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.IAM.Application.Internal.OutboundServices;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using SharedEmail = TrackLab.Shared.Domain.ValueObjects.Email;

namespace TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IHashingService hashingService)
    {
        Console.WriteLine("🌱 Iniciando seeding de la base de datos...");

        // ==================== TENANTS ====================
        var logisticTenant = new Tenant(
            "20123456789", 
            "TrackLab Logística S.A.C.", 
            "TrackLab", 
            "Av. Tecnología 123", 
            "Lima", 
            "Perú",
            new PhoneNumber("+51912345678"),
            new SharedEmail("info@tracklab.com"),
            "https://tracklab.com",
            TenantType.LOGISTIC
        );
        
        var clientTenant = new Tenant(
            "20123456790", 
            "Empresa Cliente S.A.C.", 
            "EmpresaCliente", 
            "Av. Comercial 456", 
            "Lima", 
            "Perú",
            new PhoneNumber("+51987654321"),
            new SharedEmail("ventas@empresacliente.com"),
            "https://empresacliente.com",
            TenantType.CLIENT
        );
        
        var providerTenant = new Tenant(
            "20123456791", 
            "Proveedor Industrial S.A.C.", 
            "ProveedorIndustrial", 
            "Av. Industrial 789", 
            "Lima", 
            "Perú",
            new PhoneNumber("+51954321987"),
            new SharedEmail("contacto@proveedorindustrial.com"),
            "https://proveedorindustrial.com",
            TenantType.PROVIDER
        );
        
        context.Tenants.AddRange(logisticTenant, clientTenant, providerTenant);
        await context.SaveChangesAsync();

        // ==================== USERS ====================
        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = hashingService.HashPassword("admin123"),
            Email = new SharedEmail("admin@tracklab.com"),
            FirstName = "Admin",
            LastName = "TrackLab",
            Active = true,
            TenantId = logisticTenant.Id
        };

        var clientUser = new User
        {
            Username = "cliente",
            PasswordHash = hashingService.HashPassword("cliente123"),
            Email = new SharedEmail("compras@empresacliente.com"),
            FirstName = "Juan",
            LastName = "Cliente",
            Active = true,
            TenantId = clientTenant.Id
        };

        var providerUser = new User
        {
            Username = "proveedor",
            PasswordHash = hashingService.HashPassword("proveedor123"),
            Email = new SharedEmail("ventas@proveedorindustrial.com"),
            FirstName = "María",
            LastName = "Proveedora",
            Active = true,
            TenantId = providerTenant.Id
        };

        context.Users.AddRange(adminUser, clientUser, providerUser);
        await context.SaveChangesAsync();

        // ==================== POSITIONS ====================
        var driverPosition = new Position(new CreatePositionCommand("Conductor"));
        driverPosition.SetTenantId(logisticTenant.Id);
        
        var warehousePosition = new Position(new CreatePositionCommand("Almacenero"));
        warehousePosition.SetTenantId(logisticTenant.Id);
        
        var supervisorPosition = new Position(new CreatePositionCommand("Supervisor"));
        supervisorPosition.SetTenantId(logisticTenant.Id);

        context.Positions.AddRange(driverPosition, warehousePosition, supervisorPosition);
        await context.SaveChangesAsync();

        // ==================== EMPLOYEES ====================
        var driver1 = new Employee(new CreateEmployeeCommand(
            "12345678", "carlos.conductor@tracklab.com", "Carlos", "Conductor", 
            "+51987654321", driverPosition.Id));
        driver1.SetTenantId(logisticTenant.Id);
        
        var driver2 = new Employee(new CreateEmployeeCommand(
            "87654321", "ana.conductora@tracklab.com", "Ana", "Conductora", 
            "+51912345678", driverPosition.Id));
        driver2.SetTenantId(logisticTenant.Id);
        
        var warehouseWorker = new Employee(new CreateEmployeeCommand(
            "11111111", "jose.almacenero@tracklab.com", "José", "Almacenero", 
            "+51965432187", warehousePosition.Id));
        warehouseWorker.SetTenantId(logisticTenant.Id);
        
        var supervisor = new Employee(new CreateEmployeeCommand(
            "22222222", "maria.supervisora@tracklab.com", "María", "Supervisora", 
            "+51987123456", supervisorPosition.Id));
        supervisor.SetTenantId(logisticTenant.Id);

        context.Employees.AddRange(driver1, driver2, warehouseWorker, supervisor);
        await context.SaveChangesAsync();

        // ==================== VEHICLES ====================
        var vehicle1 = new Vehicle(new CreateVehicleCommand(
            "ABC-123", 25.0m, 2, 
            new Coordinates(-12.0464, -77.0428), 15.5m));
        vehicle1.SetTenantId(logisticTenant.Id);
        
        var vehicle2 = new Vehicle(new CreateVehicleCommand(
            "XYZ-456", 30.0m, 2, 
            new Coordinates(-12.0464, -77.0428), 18.0m));
        vehicle2.SetTenantId(logisticTenant.Id);
        
        var vehicle3 = new Vehicle(new CreateVehicleCommand(
            "DEF-789", 35.0m, 2, 
            new Coordinates(-12.0464, -77.0428), 20.0m));
        vehicle3.SetTenantId(logisticTenant.Id);

        context.Vehicles.AddRange(vehicle1, vehicle2, vehicle3);
        await context.SaveChangesAsync();

        // ==================== WAREHOUSES ====================
        var logisticWarehouse = new Warehouse(new CreateWarehouseCommand(
            "Almacén Central TrackLab", EWarehouseType.Logistics, 
            -12.0464, -77.0428, "Av. Tecnología 123, Lima"));
        logisticWarehouse.SetTenantId(logisticTenant.Id);
        
        var clientWarehouse = new Warehouse(new CreateWarehouseCommand(
            "Almacén Cliente", EWarehouseType.Client, 
            -12.0500, -77.0500, "Av. Comercial 456, Lima"));
        clientWarehouse.SetTenantId(logisticTenant.Id);
        
        var providerWarehouse = new Warehouse(new CreateWarehouseCommand(
            "Almacén Proveedor", EWarehouseType.Provider, 
            -12.0400, -77.0400, "Av. Industrial 789, Lima"));
        providerWarehouse.SetTenantId(logisticTenant.Id);

        context.Warehouses.AddRange(logisticWarehouse, clientWarehouse, providerWarehouse);
        await context.SaveChangesAsync();

        // ==================== PRODUCTS ====================
        // Productos del proveedor
        var product1 = new Product(new CreateProductCommand(
            "Laptop Dell Inspiron 15", "Laptop para uso corporativo", 
            2500.00m, "PEN", "Tecnología", 50));
        product1.SetTenantId(providerTenant.Id);
        
        var product2 = new Product(new CreateProductCommand(
            "Monitor Samsung 24\"", "Monitor Full HD para oficina", 
            800.00m, "PEN", "Tecnología", 30));
        product2.SetTenantId(providerTenant.Id);
        
        var product3 = new Product(new CreateProductCommand(
            "Teclado Mecánico", "Teclado mecánico para gaming", 
            350.00m, "PEN", "Tecnología", 100));
        product3.SetTenantId(providerTenant.Id);
        
        var product4 = new Product(new CreateProductCommand(
            "Mouse Inalámbrico", "Mouse ergonómico inalámbrico", 
            120.00m, "PEN", "Tecnología", 200));
        product4.SetTenantId(providerTenant.Id);

        // Productos del cliente
        var clientProduct1 = new Product(new CreateProductCommand(
            "Producto Cliente A", "Producto manufacturado por cliente", 
            150.00m, "PEN", "Materiales", 80));
        clientProduct1.SetTenantId(clientTenant.Id);
        
        var clientProduct2 = new Product(new CreateProductCommand(
            "Producto Cliente B", "Producto procesado por cliente", 
            250.00m, "PEN", "Materiales", 60));
        clientProduct2.SetTenantId(clientTenant.Id);

        context.Products.AddRange(product1, product2, product3, product4, clientProduct1, clientProduct2);
        await context.SaveChangesAsync();

        // ==================== ORDERS ====================
        // Orden del cliente comprando productos del proveedor
        var order1 = new Order(new CreateOrderWithProductInfoCommand(
            clientTenant.Id, logisticTenant.Id, "Av. Comercial 456, Lima",
            new List<AddOrderItemWithPriceCommand>
            {
                new(product1.Id, 2, 2500.00m, "PEN"),
                new(product2.Id, 4, 800.00m, "PEN"),
                new(product3.Id, 10, 350.00m, "PEN")
            }));

        var order2 = new Order(new CreateOrderWithProductInfoCommand(
            clientTenant.Id, logisticTenant.Id, "Av. Comercial 456, Lima",
            new List<AddOrderItemWithPriceCommand>
            {
                new(product4.Id, 25, 120.00m, "PEN"),
                new(product1.Id, 1, 2500.00m, "PEN")
            }));

        // Orden del proveedor enviando productos al cliente
        var order3 = new Order(new CreateOrderWithProductInfoCommand(
            providerTenant.Id, logisticTenant.Id, "Av. Industrial 789, Lima",
            new List<AddOrderItemWithPriceCommand>
            {
                new(clientProduct1.Id, 15, 150.00m, "PEN"),
                new(clientProduct2.Id, 8, 250.00m, "PEN")
            }));

        context.Orders.AddRange(order1, order2, order3);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Base de datos inicializada con datos robustos de prueba");
        Console.WriteLine();
        Console.WriteLine("👥 USUARIOS CREADOS:");
        Console.WriteLine($"   🔑 Admin (Logística): admin / admin123");
        Console.WriteLine($"   👤 Cliente: cliente / cliente123");
        Console.WriteLine($"   🏭 Proveedor: proveedor / proveedor123");
        Console.WriteLine();
        Console.WriteLine("🏢 TENANTS CREADOS:");
        Console.WriteLine($"   🚚 Logística: {logisticTenant.GetDisplayName()} (ID: {logisticTenant.Id})");
        Console.WriteLine($"   🛒 Cliente: {clientTenant.GetDisplayName()} (ID: {clientTenant.Id})");
        Console.WriteLine($"   🏭 Proveedor: {providerTenant.GetDisplayName()} (ID: {providerTenant.Id})");
        Console.WriteLine();
        Console.WriteLine("📦 DATOS AGREGADOS:");
        Console.WriteLine($"   🚛 Vehículos: 3 (ABC-123, XYZ-456, DEF-789)");
        Console.WriteLine($"   🏪 Almacenes: 3 (Logística, Cliente, Proveedor)");
        Console.WriteLine($"   📦 Productos: 6 (4 del proveedor, 2 del cliente)");
        Console.WriteLine($"   👷 Empleados: 4 (2 conductores, 1 almacenero, 1 supervisor)");
        Console.WriteLine($"   📝 Órdenes: 3 (con productos y precios)");
        Console.WriteLine();
        Console.WriteLine("🚀 ¡Sistema listo para pruebas completas!");
    }
} 