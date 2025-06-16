namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetAllPositionsQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public string? Department { get; }
    public bool? IsActive { get; }
    
    public GetAllPositionsQuery(int? pageSize = null, int? pageNumber = null, string? department = null, bool? isActive = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Department = department;
        IsActive = isActive;
    }
} 