using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Domain.Model.Commands;

namespace TrackLab.Resources.Domain.Services;

/// <summary>
/// Interface for warehouse command service
/// </summary>
public interface IWarehouseCommandService
{
    /// <summary>
    /// Creates a new warehouse
    /// </summary>
    /// <param name="command">Create warehouse command</param>
    /// <returns>Created warehouse</returns>
    Task<Warehouse> Handle(CreateWarehouseCommand command);
    
    /// <summary>
    /// Updates an existing warehouse
    /// </summary>
    /// <param name="command">Update warehouse command</param>
    /// <returns>Updated warehouse</returns>
    Task<Warehouse> Handle(UpdateWarehouseCommand command);
    
    /// <summary>
    /// Deletes a warehouse
    /// </summary>
    /// <param name="command">Delete warehouse command</param>
    /// <returns>Task</returns>
    Task Handle(DeleteWarehouseCommand command);
}
