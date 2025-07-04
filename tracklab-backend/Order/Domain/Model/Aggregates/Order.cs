using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Entities;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

public partial class Order
{
    public long OrderId { get; private set; }
    public long TenantId { get; private set; } // CustomerId
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Customer { get; set; } = null!;
    public long LogisticsId { get; private set; } // LogisticsId
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Logistics { get; set; } = null!;
    public string ShippingAddress { get; private set; } = null!;
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = null!;
    public List<Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route> Routes { get; set; } = new();

    // Constructor requerido por EF Core
    public Order() { }

    // Constructor de dominio
    public Order(CreateOrderCommand command)
    {
        TenantId = command.CustomerId;
        LogisticsId = command.LogisticsId;
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

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
    }
} 