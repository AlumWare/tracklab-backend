using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Entities;

public partial class OrderItem
{
    public long Id { get; private set; }
    public long ProductId { get; private set; }
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Product Product { get; set; } = null!;
    public int Quantity { get; private set; }
    public Price Price { get; private set; } = null!;

    // Constructor requerido por EF Core
    public OrderItem() { }

    // Constructor de dominio
    public OrderItem(long productId, int quantity, Price price)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(quantity));
        
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public decimal GetTotalPrice()
    {
        return Price.Amount * Quantity;
    }
} 