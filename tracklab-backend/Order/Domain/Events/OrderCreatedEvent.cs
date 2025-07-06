using TrackLab.Shared.Domain.Events;

namespace Alumware.Tracklab.API.Order.Domain.Events;

/// <summary>
/// Evento que se dispara cuando se crea una nueva orden
/// </summary>
public class OrderCreatedEvent : DomainEventBase
{
    /// <summary>
    /// ID de la orden creada
    /// </summary>
    public long OrderId { get; }
    
    /// <summary>
    /// ID del cliente que creó la orden
    /// </summary>
    public long CustomerId { get; }
    
    /// <summary>
    /// Monto total de la orden
    /// </summary>
    public decimal TotalAmount { get; }
    
    /// <summary>
    /// Moneda de la orden
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Constructor del evento
    /// </summary>
    /// <param name="orderId">ID de la orden creada</param>
    /// <param name="customerId">ID del cliente</param>
    /// <param name="totalAmount">Monto total</param>
    /// <param name="currency">Moneda</param>
    public OrderCreatedEvent(long orderId, long customerId, decimal totalAmount, string currency)
        : base(version: 1)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Currency = currency;
    }
} 