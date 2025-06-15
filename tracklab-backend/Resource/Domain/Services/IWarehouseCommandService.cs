using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IWarehouseCommandService
{
    Task<Warehouse> Handle(CreateWarehouseCommand command);
    Task Handle(UpdateWarehouseInfoCommand command);
    Task Handle(DeleteWarehouseCommand command);
}
