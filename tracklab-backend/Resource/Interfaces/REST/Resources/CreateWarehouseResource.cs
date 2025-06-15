namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record CreateWarehouseResource(
    string Name,
    string Type,
    double Latitude,
    double Longitude,
    string Address
);