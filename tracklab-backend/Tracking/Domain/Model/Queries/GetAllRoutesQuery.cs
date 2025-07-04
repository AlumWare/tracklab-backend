namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetAllRoutesQuery
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public long? VehicleId { get; }
    public long? OrderId { get; }

    public GetAllRoutesQuery(int? pageSize = null, int? pageNumber = null, long? vehicleId = null, long? orderId = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        VehicleId = vehicleId;
        OrderId = orderId;
    }
} 