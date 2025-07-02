using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Entities;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

public partial class Order
{
    public long OrderId { get; private set; }
    public TenantId TenantId { get; private set; } = null!; // CustomerId
    public TenantId LogisticsId { get; private set; } = null!; // LogisticsId
    public string ShippingAddress { get; private set; } = null!;
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = null!;

    // Constructor requerido por EF Core
    public Order() { }

    // Constructor de dominio
    public Order(CreateOrderCommand command)
    {
        TenantId = new TenantId(command.CustomerId);
        LogisticsId = new TenantId(command.LogisticsId);
        ShippingAddress = command.ShippingAddress;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        OrderItems = new List<OrderItem>();
    }

    public void AddOrderItem(AddOrderItemCommand command)
    {
        var orderItem = new OrderItem(command.ProductId, command.Quantity, new Price(command.PriceAmount, command.PriceCurrency));
        OrderItems.Add(orderItem);
    }

    public void UpdateStatus(UpdateOrderStatusCommand command)
    {
        Status = command.NewStatus;
    }

    public decimal GetTotalOrderPrice()
    {
        return OrderItems.Sum(item => item.GetTotalPrice());
    }

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
} 