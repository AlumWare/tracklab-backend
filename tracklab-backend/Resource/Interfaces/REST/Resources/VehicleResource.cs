namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record VehicleResource(
    long Id,
    string LicensePlate,
    decimal LoadCapacity,
    int PaxCapacity,
    string Status,
    double Latitude,
    double Longitude,
    List<VehicleImageResource> Images
);