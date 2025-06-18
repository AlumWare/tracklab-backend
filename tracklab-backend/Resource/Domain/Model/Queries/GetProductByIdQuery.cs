namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetProductByIdQuery {
    public long Id { get; }
    public GetProductByIdQuery(long id) => Id = id;
} 