namespace Alumware.Tracklab.API.Order.Interfaces.ACL;

public interface IOrderContextFacade
{
    /// <summary>
    /// Validates if an order exists and belongs to the specified tenant
    /// </summary>
    /// <param name="orderId">Order ID to validate</param>
    /// <param name="tenantId">Tenant ID that should have access</param>
    /// <returns>True if order exists and tenant has access</returns>
    Task<bool> ValidateOrderExistsAsync(long orderId, long tenantId);
    
    /// <summary>
    /// Gets order items with product information for external validation
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <param name="tenantId">Tenant ID requesting access</param>
    /// <returns>List of order items with ProductId and Quantity</returns>
    Task<IEnumerable<OrderItemInfo>> GetOrderItemsAsync(long orderId, long tenantId);
    
    /// <summary>
    /// Gets basic order information for event publishing
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <returns>Basic order information including CustomerId and LogisticsId</returns>
    Task<OrderBasicInfo?> GetOrderBasicInfoAsync(long orderId);
    
    /// <summary>
    /// Gets the complete order aggregate by ID for route association
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <returns>Complete order aggregate if found</returns>
    Task<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order?> GetOrderByIdAsync(long orderId);
    
    /// <summary>
    /// Marks an order as completed when all containers are delivered
    /// </summary>
    /// <param name="orderId">Order ID to complete</param>
    /// <param name="tenantId">Tenant ID that should have access</param>
    Task CompleteOrderAsync(long orderId, long tenantId);
}

public record OrderItemInfo(
    long ProductId,
    int Quantity
);

public record OrderBasicInfo(
    long OrderId,
    long CustomerId,
    long LogisticsId
); 