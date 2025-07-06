using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

/// <summary>
/// Assembler to transform AddNodeResource to AddNodeCommand
/// </summary>
public static class AddNodeCommandFromResourceAssembler
{
    /// <summary>
    /// Transforms an AddNodeResource to AddNodeCommand
    /// </summary>
    /// <param name="resource">The AddNodeResource to transform</param>
    /// <param name="routeId">The route ID to add the node to</param>
    /// <returns>AddNodeCommand</returns>
    public static AddNodeCommand ToCommandFromResource(AddNodeResource resource, long routeId)
    {
        return new AddNodeCommand(
            routeId,
            resource.WarehouseId
        );
    }
} 