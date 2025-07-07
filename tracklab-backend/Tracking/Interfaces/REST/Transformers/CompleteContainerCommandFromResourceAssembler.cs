using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

/// <summary>
/// Assembler to transform CompleteContainerResource to CompleteContainerCommand
/// </summary>
public static class CompleteContainerCommandFromResourceAssembler
{
    /// <summary>
    /// Transforms a CompleteContainerResource to CompleteContainerCommand
    /// </summary>
    /// <param name="resource">The CompleteContainerResource to transform</param>
    /// <param name="containerId">The container ID to complete</param>
    /// <returns>CompleteContainerCommand</returns>
    public static CompleteContainerCommand ToCommandFromResource(CompleteContainerResource resource, long containerId)
    {
        return new CompleteContainerCommand(
            containerId,
            resource.DeliveryWarehouseId,
            resource.DeliveryDate ?? DateTime.UtcNow,
            resource.DeliveryNotes
        );
    }
} 