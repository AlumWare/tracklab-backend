using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Interfaces.ACL;

namespace Alumware.Tracklab.API.Tracking.Application.ACL;

public class TrackingContextFacade : ITrackingContextFacade
{
    private readonly IContainerRepository _containerRepository;
    private readonly ITrackingEventRepository _trackingEventRepository;

    public TrackingContextFacade(
        IContainerRepository containerRepository,
        ITrackingEventRepository trackingEventRepository)
    {
        _containerRepository = containerRepository;
        _trackingEventRepository = trackingEventRepository;
    }

    public async Task<bool> CanOrderChangeStatusAsync(long orderId, string newStatus)
    {
        var containers = await GetContainersForOrderAsync(orderId);
        
        // Business rules for order status changes based on tracking state
        return newStatus switch
        {
            "Cancelled" => await CanCancelOrderAsync(containers),
            "Shipped" => await CanShipOrderAsync(containers),
            "Delivered" => await CanDeliverOrderAsync(containers),
            "InProcess" => await CanProcessOrderAsync(containers),
            _ => true // Allow other status changes
        };
    }

    public async Task<OrderTrackingInfo> GetOrderTrackingInfoAsync(long orderId)
    {
        var containers = await GetContainersForOrderAsync(orderId);
        
        if (!containers.Any())
        {
            return new OrderTrackingInfo(
                orderId,
                0, 0, 0, 0,
                "No Containers",
                null
            );
        }

        var totalContainers = containers.Count();
        var deliveredContainers = 0;
        var inTransitContainers = 0;
        var pendingContainers = 0;
        DateTime? lastUpdated = null;

        foreach (var container in containers)
        {
            var status = DetermineContainerStatus(container);
            switch (status)
            {
                case "Delivered":
                    deliveredContainers++;
                    break;
                case "InTransit":
                    inTransitContainers++;
                    break;
                case "Pending":
                    pendingContainers++;
                    break;
            }

            // Find the latest event time
            var latestEvent = container.TrackingEvents.OrderByDescending(e => e.EventTime).FirstOrDefault();
            if (latestEvent != null && (lastUpdated == null || latestEvent.EventTime > lastUpdated))
            {
                lastUpdated = latestEvent.EventTime;
            }
        }

        var overallStatus = DetermineOverallStatus(deliveredContainers, inTransitContainers, pendingContainers, totalContainers);

        return new OrderTrackingInfo(
            orderId,
            totalContainers,
            deliveredContainers,
            inTransitContainers,
            pendingContainers,
            overallStatus,
            lastUpdated
        );
    }

    public async Task<bool> CanOrderBeDeletedAsync(long orderId)
    {
        var containers = await GetContainersForOrderAsync(orderId);
        
        // Order can be deleted if it has no containers or all containers are in initial state
        if (!containers.Any())
            return true;
            
        // Check if all containers are in "Created" state (no tracking events beyond creation)
        foreach (var container in containers)
        {
            var nonCreationEvents = container.TrackingEvents.Where(e => e.Type != EventType.Creation);
            if (nonCreationEvents.Any())
                return false;
        }
        
        return true;
    }

    public async Task<int> GetActiveContainerCountAsync(long orderId)
    {
        var containers = await GetContainersForOrderAsync(orderId);
        return containers.Count();
    }

    private async Task<IEnumerable<Container>> GetContainersForOrderAsync(long orderId)
    {
        var query = new GetAllContainersQuery(null, null, orderId, null);
        return await _containerRepository.GetAllAsync(query);
    }

    private string DetermineContainerStatus(Container container)
    {
        var events = container.TrackingEvents.OrderByDescending(e => e.EventTime).ToList();
        
        if (!events.Any())
            return "Created";
            
        var latestEvent = events.First();
        
        // Business logic to determine container status based on events
        return latestEvent.Type switch
        {
            EventType.Creation => "Created",
            EventType.Departure => "InTransit",
            EventType.Arrival => DetermineArrivalStatus(events),
            _ => "Unknown"
        };
    }
    
    private string DetermineArrivalStatus(List<TrackingEvent> events)
    {
        // If the latest event is arrival, we need to determine if it's delivered or just at a warehouse
        // This would depend on business logic - for now, assume if it's the final destination it's delivered
        // You might want to add more sophisticated logic here based on route information
        return "Delivered"; // Simplified for now
    }

    private string DetermineOverallStatus(int delivered, int inTransit, int pending, int total)
    {
        if (delivered == total)
            return "Delivered";
        if (inTransit > 0)
            return "InTransit";
        if (pending == total)
            return "Pending";
        return "PartiallyShipped";
    }

    private async Task<bool> CanCancelOrderAsync(IEnumerable<Container> containers)
    {
        // Order can be cancelled if no containers are in transit
        foreach (var container in containers)
        {
            var status = DetermineContainerStatus(container);
            if (status == "InTransit")
                return false;
        }
        return true;
    }

    private async Task<bool> CanShipOrderAsync(IEnumerable<Container> containers)
    {
        // Order can be shipped if it has containers
        return containers.Any();
    }

    private async Task<bool> CanDeliverOrderAsync(IEnumerable<Container> containers)
    {
        // Order can be delivered if all containers are delivered
        if (!containers.Any())
            return false;
            
        foreach (var container in containers)
        {
            var status = DetermineContainerStatus(container);
            if (status != "Delivered")
                return false;
        }
        return true;
    }

    private async Task<bool> CanProcessOrderAsync(IEnumerable<Container> containers)
    {
        // Order can be processed if it has containers
        return containers.Any();
    }
} 