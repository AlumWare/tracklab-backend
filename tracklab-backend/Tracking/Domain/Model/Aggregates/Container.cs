using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using System.Collections.Generic;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class Container
{
    public long ContainerId { get; private set; }
    public OrderId OrderId { get; private set; } = null!;
    public List<ShipItem> ShipItems { get; private set; } = new();
    public WarehouseId WarehouseId { get; private set; } = null!;

    public Container() { }

    public Container(CreateContainerCommand command)
    {
        OrderId = new OrderId(command.OrderId);
        WarehouseId = new WarehouseId(command.WarehouseId);
        ShipItems = command.ShipItems.Select(item => new ShipItem(item.ProductId, item.Quantity)).ToList();
    }

    public void UpdateCurrentNode(UpdateContainerNodeCommand command)
    {
        WarehouseId = new WarehouseId(command.WarehouseId);
    }
} 