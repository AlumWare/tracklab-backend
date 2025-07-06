using Alumware.Tracklab.API.Order.Interfaces.ACL;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Microsoft.Extensions.Logging;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;

public class OrderContextService : IOrderContextService
{
    private readonly IOrderContextFacade _orderContextFacade;
    private readonly IContainerRepository _containerRepository;
    private readonly ILogger<OrderContextService> _logger;

    public OrderContextService(
        IOrderContextFacade orderContextFacade,
        IContainerRepository containerRepository,
        ILogger<OrderContextService> logger)
    {
        _orderContextFacade = orderContextFacade;
        _containerRepository = containerRepository;
        _logger = logger;
    }

    public async Task<bool> ValidateOrderExistsAsync(long orderId, long tenantId)
    {
        return await _orderContextFacade.ValidateOrderExistsAsync(orderId, tenantId);
    }

    public async Task<IEnumerable<Alumware.Tracklab.API.Order.Interfaces.ACL.OrderItemInfo>> GetOrderItemsAsync(long orderId, long tenantId)
    {
        return await _orderContextFacade.GetOrderItemsAsync(orderId, tenantId);
    }

    public async Task<Dictionary<long, int>> GetAssignedQuantitiesAsync(long orderId)
    {
        var containersQuery = new GetAllContainersQuery(null, null, orderId, null);
        var containers = await _containerRepository.GetAllAsync(containersQuery);
        
        var assignedQuantities = new Dictionary<long, int>();
        
        foreach (var container in containers)
        {
            foreach (var shipItem in container.ShipItems)
            {
                if (assignedQuantities.ContainsKey(shipItem.ProductId))
                {
                    assignedQuantities[shipItem.ProductId] += (int)shipItem.Quantity;
                }
                else
                {
                    assignedQuantities[shipItem.ProductId] = (int)shipItem.Quantity;
                }
            }
        }
        
        return assignedQuantities;
    }

    public async Task<Alumware.Tracklab.API.Order.Interfaces.ACL.OrderBasicInfo?> GetOrderBasicInfoAsync(long orderId)
    {
        return await _orderContextFacade.GetOrderBasicInfoAsync(orderId);
    }

    public async Task<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order?> GetOrderByIdAsync(long orderId)
    {
        return await _orderContextFacade.GetOrderByIdAsync(orderId);
    }

    /// <summary>
    /// Marks an order as completed when all containers are delivered
    /// </summary>
    public async Task CompleteOrderAsync(long orderId, long tenantId)
    {
        try
        {
            // Use the facade to complete the order
            await _orderContextFacade.CompleteOrderAsync(orderId, tenantId);
            
            _logger.LogInformation("Order {OrderId} marked as completed for tenant {TenantId}", orderId, tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing order {OrderId} for tenant {TenantId}", orderId, tenantId);
            throw;
        }
    }
} 