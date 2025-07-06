using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Entities;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

/// <summary>
/// Route aggregate - Represents a planned route for a vehicle to deliver orders
/// </summary>
public partial class Route
{
    public long RouteId { get; private set; }
    public VehicleId VehicleId { get; private set; } = null!;
    public string RouteName { get; private set; } = string.Empty;
    public DateTime? PlannedStartDate { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    // Navigation properties
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Vehicle Vehicle { get; set; } = null!;
    public List<RouteItem> RouteItems { get; private set; } = new();
    public List<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order> Orders { get; set; } = new();

    public Route() { }

    public Route(CreateRouteCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.RouteName))
            throw new ArgumentException("Route name cannot be empty", nameof(command.RouteName));
        if (command.VehicleId <= 0)
            throw new ArgumentException("Vehicle ID must be valid", nameof(command.VehicleId));
        if (!command.WarehouseIds.Any())
            throw new ArgumentException("Route must have at least one warehouse", nameof(command.WarehouseIds));
        if (!command.OrderIds.Any())
            throw new ArgumentException("Route must serve at least one order", nameof(command.OrderIds));

        VehicleId = new VehicleId(command.VehicleId);
        RouteName = command.RouteName;
        PlannedStartDate = command.PlannedStartDate;
        Description = command.Description;
        CreatedDate = DateTimeOffset.UtcNow;
        IsActive = true;

        // Create route items for each warehouse in order
        RouteItems = command.WarehouseIds.Select(warehouseId => 
            new RouteItem(new WarehouseId(warehouseId))).ToList();

        // Initialize empty orders list - will be populated through navigation properties
        Orders = new List<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order>();
    }

    /// <summary>
    /// Adds a new warehouse node to the route
    /// </summary>
    public void AddNode(AddNodeCommand command)
    {
        if (command.WarehouseId <= 0)
            throw new ArgumentException("Warehouse ID must be valid", nameof(command.WarehouseId));

        var node = new RouteItem(new WarehouseId(command.WarehouseId));
        RouteItems.Add(node);
    }

    /// <summary>
    /// Adds an order to be served by this route
    /// </summary>
    public void AddOrder(Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (!Orders.Any(o => o.OrderId == order.OrderId))
            Orders.Add(order);
    }

    /// <summary>
    /// Marks the route as completed (all deliveries done)
    /// </summary>
    public void CompleteRoute()
    {
        IsActive = false;
    }

    /// <summary>
    /// Gets all incomplete warehouse nodes in the route
    /// </summary>
    public IEnumerable<RouteItem> GetIncompleteNodes()
    {
        return RouteItems.Where(item => !item.IsCompleted);
    }

    /// <summary>
    /// Gets all orders assigned to this route
    /// </summary>
    public IEnumerable<long> GetOrderIds()
    {
        return Orders.Select(o => o.OrderId);
    }

    /// <summary>
    /// Checks if the route is ready to start (has vehicle, warehouses, and orders)
    /// </summary>
    public bool IsReadyToStart()
    {
        return VehicleId != null && 
               RouteItems.Any() && 
               Orders.Any() && 
               IsActive;
    }
} 