using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdateVehicleInfoCommand(
    long VehicleId,
    string NewLicensePlate,
    decimal NewLoadCapacity,
    int NewPaxCapacity,
    Coordinates NewLocation
);