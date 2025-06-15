using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class CreatePositionCommandFromResourceAssembler
{
    public static CreatePositionCommand ToCommandFromResource(CreatePositionResource resource)
    {
        return new CreatePositionCommand(resource.Name);
    }
}