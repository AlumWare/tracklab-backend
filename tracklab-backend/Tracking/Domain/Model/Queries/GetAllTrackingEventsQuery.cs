namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetAllTrackingEventsQuery
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public long? ContainerId { get; }
    public long? WarehouseId { get; }
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }

    public GetAllTrackingEventsQuery(int? pageSize = null, int? pageNumber = null, long? containerId = null, long? warehouseId = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        ContainerId = containerId;
        WarehouseId = warehouseId;
        FromDate = fromDate;
        ToDate = toDate;
    }
} 