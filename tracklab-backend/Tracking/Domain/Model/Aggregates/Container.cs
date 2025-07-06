using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class Container
{
    public long ContainerId { get; private set; }
    public OrderId OrderId { get; private set; } = null!;
    public WarehouseId WarehouseId { get; private set; } = null!;
    public List<ShipItem> ShipItems { get; private set; } = new();
    public List<Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.TrackingEvent> TrackingEvents { get; set; } = new();
    public decimal TotalWeight { get; private set; }

    public Container() { }

    public Container(CreateContainerCommand command)
    {
        if (command.OrderId <= 0)
            throw new ArgumentException("El ID de la orden debe ser válido", nameof(command.OrderId));
        if (command.WarehouseId <= 0)
            throw new ArgumentException("El ID del almacén debe ser válido", nameof(command.WarehouseId));
        if (command.TotalWeight <= 0)
            throw new ArgumentException("El peso total debe ser mayor a 0", nameof(command.TotalWeight));
        
        OrderId = new OrderId(command.OrderId);
        WarehouseId = new WarehouseId(command.WarehouseId);
        TotalWeight = command.TotalWeight;
        // Los ShipItems se establecerán desde el servicio usando los productos de la orden
        ShipItems = new List<ShipItem>();
    }

    public void SetShipItemsFromOrder(IEnumerable<Alumware.Tracklab.API.Order.Domain.Model.Entities.OrderItem>? orderItems)
    {
        if (orderItems == null || !orderItems.Any())
        {
            ShipItems = new List<ShipItem>();
            return;
        }
        
        ShipItems = orderItems.Select(item => new ShipItem(
            item.ProductId, 
            item.Quantity, 
            1.0m // Peso unitario por defecto de 1kg
        )).ToList();
        
        // El TotalWeight ya está establecido desde el comando, no se recalcula
    }

    public void UpdateCurrentNode(UpdateContainerNodeCommand command)
    {
        if (command.WarehouseId <= 0)
            throw new ArgumentException("El ID del almacén debe ser válido", nameof(command.WarehouseId));
        
        WarehouseId = new WarehouseId(command.WarehouseId);
    }

    public int GetTotalItems()
    {
        return ShipItems.Sum(item => (int)item.Quantity);
    }

    public bool HasProduct(long productId)
    {
        return ShipItems.Any(item => item.ProductId == productId);
    }

    public ShipItem? GetProductItem(long productId)
    {
        return ShipItems.FirstOrDefault(item => item.ProductId == productId);
    }
} 