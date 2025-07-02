using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Entities;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class Route
{
    public long RouteId { get; private set; }
    public List<RouteItem> RouteItems { get; private set; } = new();
    public List<OrderId> Orders { get; private set; } = new();
    public VehicleId VehicleId { get; private set; } = null!;

    public Route() { }

    public Route(CreateRouteCommand command)
    {
        VehicleId = new VehicleId(command.VehicleId);
        RouteItems = new List<RouteItem>();
        Orders = new List<OrderId>();
    }

    public void AddNode(AddNodeCommand command)
    {
        var node = new RouteItem(new WarehouseId(command.WarehouseId));
        RouteItems.Add(node);
    }

    public void AddOrder(AddOrderToRouteCommand command)
    {
        var orderId = new OrderId(command.OrderId);
        if (!Orders.Contains(orderId))
            Orders.Add(orderId);
    }
} 