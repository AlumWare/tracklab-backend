namespace TrackLab.Shared.Domain.ValueObjects;

/// <summary>
///     Tenant identifier value object
/// </summary>
/// <param name="Value">
///     The tenant identifier
/// </param>
public readonly record struct TenantId(int Value)
{
    public override string ToString() => Value.ToString();
}