namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

/// <summary>
/// Command for completing an order when all containers are delivered
/// </summary>
public record CompleteOrderCommand(long OrderId); 