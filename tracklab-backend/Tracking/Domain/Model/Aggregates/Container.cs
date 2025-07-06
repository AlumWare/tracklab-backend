using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class Container
{
    public long ContainerId { get; private set; }
    public OrderId OrderId { get; private set; } = null!;
    public Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order Order { get; set; } = null!;
    public WarehouseId WarehouseId { get; private set; } = null!;
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse Warehouse { get; set; } = null!;
    public List<ShipItem> ShipItems { get; private set; } = new();
    public List<Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.TrackingEvent> TrackingEvents { get; set; } = new();
    public decimal TotalWeight { get; private set; }

    public Container() { }

    public Container(CreateContainerCommand command)
    {
        OrderId = new OrderId(command.OrderId);
        WarehouseId = new WarehouseId(command.WarehouseId);
        ShipItems = command.ShipItems.Select(item => new ShipItem(item.ProductId, item.Quantity, item.UnitWeight)).ToList();
        TotalWeight = ShipItems.Sum(item => item.Quantity * item.UnitWeight);
    }

    public void UpdateCurrentNode(UpdateContainerNodeCommand command)
    {
        WarehouseId = new WarehouseId(command.WarehouseId);
    }
} 