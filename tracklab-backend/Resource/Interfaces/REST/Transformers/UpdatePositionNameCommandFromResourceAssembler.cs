using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class UpdatePositionNameCommandFromResourceAssembler
{
    public static UpdatePositionNameCommand ToCommandFromResource(long id, UpdatePositionResource resource)
    {
        return new UpdatePositionNameCommand(id, resource.NewName);
    }
}