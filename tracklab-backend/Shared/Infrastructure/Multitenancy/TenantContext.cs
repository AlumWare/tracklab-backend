namespace TrackLab.Shared.Infrastructure.Multitenancy;

public class TenantContext : ITenantContext
{
    private static readonly AsyncLocal<long?> _currentTenantId = new();

    public long? CurrentTenantId => _currentTenantId.Value;

    public bool HasTenant => _currentTenantId.Value != null;

    public void SetTenant(long tenantId)
    {
        _currentTenantId.Value = tenantId;
    }

    public void ClearTenant()
    {
        _currentTenantId.Value = null;
    }
}
