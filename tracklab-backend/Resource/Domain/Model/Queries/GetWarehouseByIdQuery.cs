namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetWarehouseByIdQuery {
    public long Id { get; }
    public GetWarehouseByIdQuery(long id) => Id = id;
} 