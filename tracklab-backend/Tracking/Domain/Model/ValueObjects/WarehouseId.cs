using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

[Owned]
public record WarehouseId
{
    public long Value { get; private set; }

    public WarehouseId() { }
    public WarehouseId(long value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID del almacén debe ser mayor que 0", nameof(value));
        Value = value;
    }
    public override string ToString() => Value.ToString();
    public static implicit operator long(WarehouseId id) => id.Value;
    public static explicit operator WarehouseId(long value) => new(value);
} 