namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetAllContainersQuery
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public long? OrderId { get; }
    public long? WarehouseId { get; }

    public GetAllContainersQuery(int? pageSize = null, int? pageNumber = null, long? orderId = null, long? warehouseId = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        OrderId = orderId;
        WarehouseId = warehouseId;
    }
} 