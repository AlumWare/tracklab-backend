using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.ACL;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Alumware.Tracklab.API.Order.Application.ACL;

public class OrderContextFacade : IOrderContextFacade
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderCommandService _orderCommandService;
    private readonly ILogger<OrderContextFacade> _logger;

    public OrderContextFacade(
        IOrderRepository orderRepository,
        IOrderCommandService orderCommandService,
        ILogger<OrderContextFacade> logger)
    {
        _orderRepository = orderRepository;
        _orderCommandService = orderCommandService;
        _logger = logger;
    }

    public async Task<bool> ValidateOrderExistsAsync(long orderId, long tenantId)
    {
        // Usar el método ACL específico que bypassa el filtro de tenant
        var order = await _orderRepository.GetByIdForACLAsync(orderId);
        
        // Verificar que la orden existe y pertenece al tenant actual (como customer) 
        // o que el tenant actual es la empresa logística
        return order != null && (order.TenantId == tenantId || order.LogisticsId == tenantId);
    }

    public async Task<IEnumerable<OrderItemInfo>> GetOrderItemsAsync(long orderId, long tenantId)
    {
        // Primero validar que tenemos acceso a la orden
        if (!await ValidateOrderExistsAsync(orderId, tenantId))
        {
            throw new UnauthorizedAccessException($"No tiene acceso a la orden {orderId}");
        }

        // Usar el método ACL específico que bypassa el filtro de tenant
        var order = await _orderRepository.GetByIdForACLAsync(orderId);
        
        if (order?.OrderItems == null)
        {
            return Enumerable.Empty<OrderItemInfo>();
        }

        return order.OrderItems.Select(item => new OrderItemInfo(
            item.ProductId,
            item.Quantity
        ));
    }

    public async Task<OrderBasicInfo?> GetOrderBasicInfoAsync(long orderId)
    {
        // Usar el método ACL específico que bypassa el filtro de tenant
        var order = await _orderRepository.GetByIdForACLAsync(orderId);
        
        if (order == null)
        {
            return null;
        }

        return new OrderBasicInfo(
            order.OrderId,
            order.TenantId,
            order.LogisticsId
        );
    }

    public async Task<Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order?> GetOrderByIdAsync(long orderId)
    {
        // Usar el método ACL específico que bypassa el filtro de tenant
        return await _orderRepository.GetByIdForACLAsync(orderId);
    }

    public async Task CompleteOrderAsync(long orderId, long tenantId)
    {
        try
        {
            // Validate access to the order
            if (!await ValidateOrderExistsAsync(orderId, tenantId))
            {
                throw new UnauthorizedAccessException($"No tiene acceso a la orden {orderId}");
            }

            // Get the order using ACL method
            var order = await _orderRepository.GetByIdForACLAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order {orderId} not found");
            }

            // Complete the order (assuming there's a method to mark as completed)
            var completeCommand = new CompleteOrderCommand(orderId);
            await _orderCommandService.CompleteOrderAsync(completeCommand);
            
            _logger.LogInformation("Order {OrderId} completed successfully by tenant {TenantId}", orderId, tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing order {OrderId} for tenant {TenantId}", orderId, tenantId);
            throw;
        }
    }
} 