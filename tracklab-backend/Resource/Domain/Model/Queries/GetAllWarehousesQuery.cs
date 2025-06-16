namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetAllWarehousesQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public string? Location { get; }
    public string? Type { get; }
    public bool? IsActive { get; }
    
    public GetAllWarehousesQuery(int? pageSize = null, int? pageNumber = null, string? location = null, string? type = null, bool? isActive = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Location = location;
        Type = type;
        IsActive = isActive;
    }
} 