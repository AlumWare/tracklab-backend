using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

[Owned]
public record ProductId
{
    public long Value { get; private set; }

    public ProductId() { } // Requerido por EF Core

    public ProductId(long value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID del producto debe ser mayor que 0", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator long(ProductId productId) => productId.Value;
    public static explicit operator ProductId(long value) => new(value);
} 