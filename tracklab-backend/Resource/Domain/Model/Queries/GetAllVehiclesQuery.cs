namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetAllVehiclesQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public string? Status { get; }
    public string? Type { get; }
    public string? LicensePlate { get; }
    
    public GetAllVehiclesQuery(int? pageSize = null, int? pageNumber = null, string? status = null, string? type = null, string? licensePlate = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Status = status;
        Type = type;
        LicensePlate = licensePlate;
    }
} 