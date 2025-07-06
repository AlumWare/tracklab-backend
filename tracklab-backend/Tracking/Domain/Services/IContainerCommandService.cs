using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface IContainerCommandService
{
    Task<Container> CreateAsync(CreateContainerCommand command);
    Task<Container> UpdateCurrentNodeAsync(UpdateContainerNodeCommand command);
    Task<Container> CompleteContainerAsync(CompleteContainerCommand command);
} 