using Microsoft.Extensions.Logging;
using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Events;

namespace Alumware.Tracklab.API.Order.Application.Internal.EventHandlers;

/// <summary>
/// Handler para el evento OrderCreatedEvent
/// </summary>
public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    
    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Maneja el evento OrderCreatedEvent
    /// </summary>
    /// <param name="notification">Evento de orden creada</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling OrderCreatedEvent for Order {OrderId}, Customer {CustomerId}, Amount {TotalAmount} {Currency}",
            notification.OrderId, 
            notification.CustomerId, 
            notification.TotalAmount, 
            notification.Currency
        );

        // Aquí puedes agregar lógica de negocio como:
        // - Enviar notificación al cliente
        // - Crear una entrada de auditoría
        // - Iniciar procesos de inventario
        // - Enviar email de confirmación
        // - Etc.
        
        // Ejemplo: Log de auditoría
        await LogOrderCreationAsync(notification);
        
        // Ejemplo: Notificar a otros sistemas
        await NotifyExternalSystemsAsync(notification);
        
        _logger.LogInformation("OrderCreatedEvent handled successfully for Order {OrderId}", notification.OrderId);
    }

    private async Task LogOrderCreationAsync(OrderCreatedEvent orderCreatedEvent)
    {
        // Simulación de logging de auditoría
        await Task.Delay(10); // Simular operación asíncrona
        
        _logger.LogInformation(
            "AUDIT: Order {OrderId} created by Customer {CustomerId} for {TotalAmount} {Currency} at {CreatedAt}",
            orderCreatedEvent.OrderId,
            orderCreatedEvent.CustomerId,
            orderCreatedEvent.TotalAmount,
            orderCreatedEvent.Currency,
            orderCreatedEvent.OccurredOn
        );
    }

    private async Task NotifyExternalSystemsAsync(OrderCreatedEvent orderCreatedEvent)
    {
        // Simulación de notificación a sistemas externos
        await Task.Delay(50); // Simular llamada a API externa
        
        _logger.LogInformation(
            "EXTERNAL: Notified external systems about Order {OrderId} creation",
            orderCreatedEvent.OrderId
        );
    }
} 