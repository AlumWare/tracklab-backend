namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record UpdateWarehouseResource(
    string Name,
    double Latitude,
    double Longitude,
    string Address
);
