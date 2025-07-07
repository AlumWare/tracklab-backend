using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Entities;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

public partial class Order
{
    public long OrderId { get; private set; }
    public long TenantId { get; private set; } // CustomerId
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Customer { get; set; } = null!;
    public string ShippingAddress { get; private set; } = null!;
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = null!;
    public List<Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route> Routes { get; set; } = new();
    public long? VehicleId { get; private set; } // Nuevo campo para vehículo asignado
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public long LogisticsId { get; private set; }

    // Constructor requerido por EF Core
    public Order() { }

    // Constructor de dominio SOLO con información de productos
    public Order(CreateOrderWithProductInfoCommand command)
    {
        OrderId = 0; // Será asignado por EF Core
        LogisticsId = command.LogisticsId;
        ShippingAddress = command.ShippingAddress;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        // Crear los items de la orden
        OrderItems = command.Items.Select(item => new OrderItem(
            item.ProductId,
            item.Quantity,
            new Price(item.PriceAmount, item.PriceCurrency)
        )).ToList();
    }

    public void AddOrderItem(AddOrderItemCommand command)
    {
        var orderItem = new OrderItem(command.ProductId, command.Quantity, new Price(command.PriceAmount, command.PriceCurrency));
        OrderItems.Add(orderItem);
    }

    public void UpdateStatus(UpdateOrderStatusCommand command)
    {
        Status = command.NewStatus;
        UpdateTimestamp();
    }

    public decimal GetTotalOrderPrice()
    {
        return OrderItems?.Sum(item => item.GetTotalPrice()) ?? 0m;
    }

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
        UpdateTimestamp();
    }

    public void AssignLogisticsAndVehicle(long logisticsId, long vehicleId)
    {
        // Eliminado LogisticsId y Logistics de la lógica de asignación
        VehicleId = vehicleId;
        Status = OrderStatus.InProcess; // Cambia el estado a En proceso
        UpdateTimestamp();
    }

    public void AssignVehicle(long vehicleId)
    {
        VehicleId = vehicleId;
        UpdateTimestamp();
    }

    public void SetRoute(long vehicleId, List<long> warehouses)
    {
        VehicleId = vehicleId;
        // Aquí deberías crear o actualizar la entidad Route asociada a la orden
        // y asignar la lista de almacenes (warehouses) en orden.
        // Este es un placeholder para la lógica real de rutas.
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
} 