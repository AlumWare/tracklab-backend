using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

[Owned]
public record OrderId
{
    public long Value { get; private set; }

    public OrderId() { }
    public OrderId(long value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID de la orden debe ser mayor que 0", nameof(value));
        Value = value;
    }
    public override string ToString() => Value.ToString();
    public static implicit operator long(OrderId id) => id.Value;
    public static explicit operator OrderId(long value) => new(value);
} 