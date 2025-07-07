using Alumware.Tracklab.API.Order.Interfaces.ACL;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;

/// <summary>
/// Anti-corruption layer for OrderContext services
/// </summary>
public interface IOrderContextService
{
    /// <summary>
    /// Validates if an order exists and is accessible by the given tenant
    /// </summary>
    Task<bool> ValidateOrderExistsAsync(long orderId, long tenantId);
    
    /// <summary>
    /// Gets order items for validation
    /// </summary>
    Task<IEnumerable<Alumware.Tracklab.API.Order.Interfaces.ACL.OrderItemInfo>> GetOrderItemsAsync(long orderId, long tenantId);
    
    /// <summary>
    /// Gets already assigned quantities for an order
    /// </summary>
    Task<Dictionary<long, int>> GetAssignedQuantitiesAsync(long orderId);
    
    /// <summary>
    /// Gets basic order information for event publishing
    /// </summary>
    Task<Alumware.Tracklab.API.Order.Interfaces.ACL.OrderBasicInfo?> GetOrderBasicInfoAsync(long orderId);
    
    /// <summary>
    /// Gets the complete order aggregate by ID for route association
    /// </summary>
    Task<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order?> GetOrderByIdAsync(long orderId);
    
    /// <summary>
    /// Marks an order as completed when all containers are delivered
    /// </summary>
    Task CompleteOrderAsync(long orderId, long tenantId);
}

/// <summary>
/// Information about order items for validation
/// </summary>
public record OrderItemInfo(
    long ProductId,
    int Quantity
); 