namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record RemoveVehicleImageCommand(
    long VehicleId,
    string PublicId
); 