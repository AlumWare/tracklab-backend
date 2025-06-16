using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record TenantId
{
    [Column("tenant_id")] // Especifica el nombre de columna exacto
    public long Value { get; init; }

    public TenantId() { } 

    public TenantId(long value)
    {
        if (value <= 0) throw new ArgumentException("Tenant ID must be positive");
        Value = value;
    }

    public override string ToString() => Value.ToString();

    // Conversiones implícitas para facilitar el uso
    public static implicit operator long(TenantId tenantId) => tenantId.Value;
    public static implicit operator TenantId(long value) => new(value);
}