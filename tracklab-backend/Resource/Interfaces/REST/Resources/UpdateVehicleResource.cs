namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record UpdateVehicleResource(
    string NewLicensePlate,
    decimal NewLoadCapacity,
    int NewPaxCapacity,
    double NewLatitude,
    double NewLongitude
);