namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetVehicleByIdQuery {
    public long Id { get; }
    public GetVehicleByIdQuery(long id) => Id = id;
} 