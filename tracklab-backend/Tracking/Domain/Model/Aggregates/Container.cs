using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

/// <summary>
/// Container aggregate representing a package containing products for delivery
/// </summary>
public partial class Container
{
    public long ContainerId { get; private set; }
    public OrderId OrderId { get; private set; } = null!;
    public WarehouseId WarehouseId { get; private set; } = null!;
    public List<ShipItem> ShipItems { get; private set; } = new();
    public List<Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.TrackingEvent> TrackingEvents { get; set; } = new();
    public decimal TotalWeight { get; private set; }
    public QrCode? QrCode { get; private set; }
    
    // Completion tracking
    public bool IsCompleted { get; private set; } = false;
    public DateTime? CompletedAt { get; private set; }
    public string? CompletionNotes { get; private set; }

    public Container() { }

    public Container(CreateContainerCommand command)
    {
        if (command.OrderId <= 0)
            throw new ArgumentException("El ID de la orden debe ser válido", nameof(command.OrderId));
        if (command.WarehouseId <= 0)
            throw new ArgumentException("El ID del almacén debe ser válido", nameof(command.WarehouseId));
        if (command.TotalWeight < 0)
            throw new ArgumentException("El peso total no puede ser negativo", nameof(command.TotalWeight));
        if (command.ShipItems == null || !command.ShipItems.Any())
            throw new ArgumentException("El contenedor debe tener al menos un producto", nameof(command.ShipItems));
        
        OrderId = new OrderId(command.OrderId);
        WarehouseId = new WarehouseId(command.WarehouseId);
        TotalWeight = command.TotalWeight;
        
        // Establecer los ShipItems desde el comando
        ShipItems = command.ShipItems.Select(item => new ShipItem(
            item.ProductId,
            (uint)item.Quantity,
            item.UnitWeight
        )).ToList();
        
        // Si TotalWeight es 0, calcularlo automáticamente
        if (TotalWeight == 0)
        {
            TotalWeight = ShipItems.Sum(item => item.UnitWeight * item.Quantity);
        }
    }

    public void UpdateCurrentNode(UpdateContainerNodeCommand command)
    {
        if (command.WarehouseId <= 0)
            throw new ArgumentException("El ID del almacén debe ser válido", nameof(command.WarehouseId));
        
        WarehouseId = new WarehouseId(command.WarehouseId);
    }

    public void AssignQrCode(QrCode qrCode)
    {
        QrCode = qrCode ?? throw new ArgumentNullException(nameof(qrCode));
    }
    
    /// <summary>
    /// Marks the container as completed when delivered at a CLIENT warehouse
    /// </summary>
    public void CompleteContainer(CompleteContainerCommand command)
    {
        if (IsCompleted)
            throw new InvalidOperationException("El contenedor ya está completado");
            
        if (command.DeliveryWarehouseId <= 0)
            throw new ArgumentException("El ID del almacén de entrega debe ser válido", nameof(command.DeliveryWarehouseId));
            
        IsCompleted = true;
        CompletedAt = command.DeliveryDate;
        CompletionNotes = command.DeliveryNotes;
        
        // Update current warehouse to delivery location
        WarehouseId = new WarehouseId(command.DeliveryWarehouseId);
    }
    
    /// <summary>
    /// Gets the total quantity of items in the container
    /// </summary>
    public int GetTotalItems()
    {
        return ShipItems.Sum(item => (int)item.Quantity);
    }

    /// <summary>
    /// Checks if the container has a specific product
    /// </summary>
    public bool HasProduct(long productId)
    {
        return ShipItems.Any(item => item.ProductId == productId);
    }

    /// <summary>
    /// Gets the ship item for a specific product
    /// </summary>
    public ShipItem? GetProductItem(long productId)
    {
        return ShipItems.FirstOrDefault(item => item.ProductId == productId);
    }
    
    /// <summary>
    /// Gets the quantity of a specific product in the container
    /// </summary>
    public int GetProductQuantity(long productId)
    {
        var item = GetProductItem(productId);
        return item != null ? (int)item.Quantity : 0;
    }
    
    /// <summary>
    /// Gets all products and their quantities in the container
    /// </summary>
    public Dictionary<long, int> GetProductQuantities()
    {
        return ShipItems.ToDictionary(item => item.ProductId, item => (int)item.Quantity);
    }
} 