using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdateVehicleStatusCommand(
    long VehicleId,
    EVehicleStatus NewStatus
);