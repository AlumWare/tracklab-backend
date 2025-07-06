using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IVehicleCommandService
{
    Task<Vehicle> Handle(CreateVehicleCommand command);
    Task Handle(UpdateVehicleStatusCommand command);
    Task Handle(UpdateVehicleInfoCommand command);
    Task Handle(DeleteVehicleCommand command);
    Task Handle(AddVehicleImageCommand command);
    Task Handle(RemoveVehicleImageCommand command);
}