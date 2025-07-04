namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetRouteByIdQuery {
    public long Id { get; }
    public GetRouteByIdQuery(long id) => Id = id;
} 