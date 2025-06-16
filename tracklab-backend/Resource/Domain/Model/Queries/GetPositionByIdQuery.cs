namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetPositionByIdQuery {
    public long Id { get; }
    public GetPositionByIdQuery(long id) => Id = id;
} 