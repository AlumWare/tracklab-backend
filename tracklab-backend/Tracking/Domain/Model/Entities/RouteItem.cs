using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Entities;

public partial class RouteItem
{
    public long Id { get; private set; }
    public WarehouseId WarehouseId { get; private set; } = null!;
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse Warehouse { get; set; } = null!;
    public long RouteId { get; set; }
    public Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route Route { get; set; } = null!;
    public DateTime? CompletedAt { get; private set; }
    public bool IsCompleted { get; private set; }

    public RouteItem() { }

    public RouteItem(WarehouseId warehouseId)
    {
        WarehouseId = warehouseId;
        IsCompleted = false;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
} 