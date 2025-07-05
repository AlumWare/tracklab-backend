using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Domain.Services;

namespace Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public class TrackingEventCommandService : ITrackingEventCommandService
{
    private readonly ITrackingEventRepository _trackingEventRepository;
    private readonly IOrderRepository _orderRepository;

    public TrackingEventCommandService(ITrackingEventRepository trackingEventRepository, IOrderRepository orderRepository)
    {
        _trackingEventRepository = trackingEventRepository;
        _orderRepository = orderRepository;
    }

    public async Task<TrackingEvent> RegisterTrackingEventAsync(RegisterTrackingEventCommand command)
    {
        var order = await _orderRepository.GetByIdAsync(new GetOrderByIdQuery(command.OrderId));
        if (order == null)
            throw new Exception("Orden no encontrada");

        // Validar estado de la orden
        if (order.Status != OrderStatus.InProcess && order.Status != OrderStatus.Shipped)
            throw new Exception("No se puede registrar tracking para una orden que no está en proceso o despachada");

        // Validar secuencia de eventos
        var lastEvent = await _trackingEventRepository.GetLastByOrderIdAsync(command.OrderId);
        int nextSequence = (lastEvent?.Sequence ?? 0) + 1;
        if (lastEvent != null && command.EventTime <= lastEvent.EventTime)
            throw new Exception("La fecha del evento debe ser posterior al último evento registrado");

        // Validar tipo de evento (puedes expandir según lógica de negocio)
        if (string.IsNullOrWhiteSpace(command.EventType))
            throw new Exception("Tipo de evento requerido");

        var trackingEvent = new TrackingEvent
        {
            OrderId = command.OrderId,
            EventType = command.EventType,
            WarehouseId = command.WarehouseId,
            EventTime = command.EventTime,
            Sequence = nextSequence
        };
        await _trackingEventRepository.AddAsync(trackingEvent);
        return trackingEvent;
    }

    public async Task<List<TrackingEvent>> GetTrackingEventsAsync(long orderId)
    {
        return await _trackingEventRepository.GetByOrderIdAsync(orderId);
    }
} 