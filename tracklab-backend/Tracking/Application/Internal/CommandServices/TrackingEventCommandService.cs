using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Tracking.Domain.Events;
using Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class TrackingEventCommandService : ITrackingEventCommandService
{
    private readonly ITrackingEventRepository _trackingEventRepository;
    private readonly IContainerRepository _containerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IOrderContextService _orderContextService;
    private readonly ILogger<TrackingEventCommandService> _logger;

    public TrackingEventCommandService(
        ITrackingEventRepository trackingEventRepository,
        IContainerRepository containerRepository,
        IUnitOfWork unitOfWork,
        IDomainEventDispatcher eventDispatcher,
        IOrderContextService orderContextService,
        ILogger<TrackingEventCommandService> logger)
    {
        _trackingEventRepository = trackingEventRepository;
        _containerRepository = containerRepository;
        _unitOfWork = unitOfWork;
        _eventDispatcher = eventDispatcher;
        _orderContextService = orderContextService;
        _logger = logger;
    }

    public async Task<TrackingEvent> CreateAsync(CreateTrackingEventCommand command)
    {
        var trackingEvent = new TrackingEvent(command);
        await _trackingEventRepository.AddAsync(trackingEvent);
        
        // If this is a delivery event, publish domain events
        if (command.Type == EventType.Delivered)
        {
            await HandleDeliveryEvent(command);
        }
        
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return trackingEvent;
    }

    private async Task HandleDeliveryEvent(CreateTrackingEventCommand command)
    {
        try
        {
            // Get container information
            var container = await _containerRepository.GetByIdAsync(new Alumware.Tracklab.API.Tracking.Domain.Model.Queries.GetContainerByIdQuery(command.ContainerId));
            if (container == null)
            {
                _logger.LogWarning("Container not found for delivery event: {ContainerId}", command.ContainerId);
                return;
            }

            // Get order information from OrderContext through ACL
            var orderInfo = await _orderContextService.GetOrderBasicInfoAsync(container.OrderId.Value);
            if (orderInfo == null)
            {
                _logger.LogWarning("Order not found for delivery event: {OrderId}", container.OrderId.Value);
                return;
            }

            // Publish ContainerDeliveredEvent
            var containerDeliveredEvent = new ContainerDeliveredEvent(
                command.ContainerId,
                container.OrderId.Value,
                orderInfo.CustomerId, // Customer tenant ID from order
                orderInfo.LogisticsId, // Logistics tenant ID from order
                $"Warehouse {command.WarehouseId}", // Delivery location
                command.EventTime,
                container.TotalWeight
            );
            
            await _eventDispatcher.PublishAsync(containerDeliveredEvent);
            
            // Check if all containers for this order are delivered
            await CheckAndPublishOrderCompletedEvent(container.OrderId.Value, orderInfo.CustomerId, orderInfo.LogisticsId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling delivery event for container {ContainerId}", command.ContainerId);
            throw;
        }
    }

    private async Task CheckAndPublishOrderCompletedEvent(long orderId, long customerId, long logisticsId)
    {
        try
        {
            // Get all containers for this order
            var allContainers = await _containerRepository.GetByOrderIdAsync(orderId);
            
            if (!allContainers.Any())
            {
                _logger.LogWarning("No containers found for order {OrderId}", orderId);
                return;
            }

            // Check if all containers have been delivered
            var deliveredContainers = new List<Container>();
            foreach (var container in allContainers)
            {
                var deliveryEvents = await _trackingEventRepository.GetByContainerIdAsync(container.ContainerId);
                if (deliveryEvents.Any(e => e.Type == EventType.Delivered))
                {
                    deliveredContainers.Add(container);
                }
            }

            // If all containers are delivered, publish OrderCompletedEvent
            if (deliveredContainers.Count == allContainers.Count())
            {
                var totalWeight = allContainers.Sum(c => c.TotalWeight);
                var completedAt = DateTime.UtcNow;
                
                var deliveryStats = $"Entrega completada: {deliveredContainers.Count} contenedores, Peso total: {totalWeight:F2} kg";
                
                var orderCompletedEvent = new OrderCompletedEvent(
                    orderId,
                    customerId,
                    logisticsId,
                    deliveredContainers.Count,
                    totalWeight,
                    completedAt,
                    deliveryStats
                );
                
                await _eventDispatcher.PublishAsync(orderCompletedEvent);
                
                _logger.LogInformation("Order {OrderId} completed - all {ContainerCount} containers delivered", 
                    orderId, deliveredContainers.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking order completion for order {OrderId}", orderId);
            throw;
        }
    }
} 