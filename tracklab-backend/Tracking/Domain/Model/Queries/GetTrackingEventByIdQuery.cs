namespace Alumware.Tracklab.API.Tracking.Domain.Model.Queries;

public class GetTrackingEventByIdQuery {
    public long Id { get; }
    public GetTrackingEventByIdQuery(long id) => Id = id;
} 