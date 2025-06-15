using Microsoft.EntityFrameworkCore;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record TenantId
{
    public long Value { get; init; }

    public TenantId() { } // requerido por EF

    public TenantId(long value)
    {
        if (value <= 0) throw new ArgumentException("Tenant ID must be positive");
        Value = value;
    }

    public override string ToString() => Value.ToString();
}