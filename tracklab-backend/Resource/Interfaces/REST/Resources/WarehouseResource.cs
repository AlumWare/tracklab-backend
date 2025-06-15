namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record WarehouseResource(
    long Id,
    string Name,
    string Type,
    double Latitude,
    double Longitude,
    string Address
);

