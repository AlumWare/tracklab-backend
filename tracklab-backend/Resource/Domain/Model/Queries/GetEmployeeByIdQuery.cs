namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetEmployeeByIdQuery {
    public long Id { get; }
    public GetEmployeeByIdQuery(long id) => Id = id;
} 