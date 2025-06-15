using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record CreateVehicleCommand(
    string LicensePlate,
    decimal LoadCapacity,
    int PaxCapacity,
    Coordinates Location
);
