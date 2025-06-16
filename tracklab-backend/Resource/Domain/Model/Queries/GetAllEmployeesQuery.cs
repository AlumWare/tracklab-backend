namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetAllEmployeesQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public string? Status { get; }
    public string? Position { get; }
    
    public GetAllEmployeesQuery(int? pageSize = null, int? pageNumber = null, string? status = null, string? position = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Status = status;
        Position = position;
    }
} 