namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetContainerByIdQuery {
    public long Id { get; }
    public GetContainerByIdQuery(long id) => Id = id;
} 