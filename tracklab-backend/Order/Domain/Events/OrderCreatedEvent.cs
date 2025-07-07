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
    /// ID de la empresa logística asignada
    /// </summary>
    public long LogisticsId { get; }
    
    /// <summary>
    /// Dirección de envío
    /// </summary>
    public string ShippingAddress { get; }
    
    /// <summary>
    /// Monto total de la orden
    /// </summary>
    public decimal TotalAmount { get; }
    
    /// <summary>
    /// Moneda de la orden
    /// </summary>
    public string Currency { get; }
    
    /// <summary>
    /// Número total de artículos en la orden
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// Constructor del evento
    /// </summary>
    /// <param name="orderId">ID de la orden creada</param>
    /// <param name="customerId">ID del cliente</param>
    /// <param name="logisticsId">ID de la empresa logística</param>
    /// <param name="shippingAddress">Dirección de envío</param>
    /// <param name="totalAmount">Monto total</param>
    /// <param name="currency">Moneda</param>
    /// <param name="totalItems">Número total de artículos</param>
    public OrderCreatedEvent(
        long orderId, 
        long customerId, 
        long logisticsId,
        string shippingAddress,
        decimal totalAmount, 
        string currency,
        int totalItems)
        : base(version: 1)
    {
        OrderId = orderId;
        CustomerId = customerId;
        LogisticsId = logisticsId;
        ShippingAddress = shippingAddress;
        TotalAmount = totalAmount;
        Currency = currency;
        TotalItems = totalItems;
    }
} 