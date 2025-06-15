using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdateWarehouseInfoCommand(
    long WarehouseId,
    string Name,
    double Latitude,
    double Longitude,
    string Address
);