using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class PositionResourceFromEntityAssembler
{
    public static PositionResource ToResourceFromEntity(Position position)
    {
        return new PositionResource(position.Id, position.Name);
    }
}