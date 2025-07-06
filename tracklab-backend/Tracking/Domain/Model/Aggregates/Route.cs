using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Entities;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class Route
{
    public long RouteId { get; private set; }
    public VehicleId VehicleId { get; private set; } = null!;
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Vehicle Vehicle { get; set; } = null!;
    public List<RouteItem> RouteItems { get; private set; } = new();
    public List<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order> Orders { get; set; } = new();

    public Route() { }

    public Route(CreateRouteCommand command)
    {
        VehicleId = new VehicleId(command.VehicleId);
        RouteItems = new List<RouteItem>();
        Orders = new List<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order>();
    }

    public void AddNode(AddNodeCommand command)
    {
        var node = new RouteItem(new WarehouseId(command.WarehouseId));
        RouteItems.Add(node);
    }

    public void AddOrder(Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order order)
    {
        if (!Orders.Any(o => o.OrderId == order.OrderId))
            Orders.Add(order);
    }
} 