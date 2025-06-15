using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record CreateWarehouseCommand(
    string Name,
    EWarehouseType Type,
    double Latitude,
    double Longitude,
    string Address
);