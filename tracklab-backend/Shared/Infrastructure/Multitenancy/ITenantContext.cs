using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Shared.Infrastructure.Multitenancy;

public interface ITenantContext
{
    long? CurrentTenantId { get; }
    bool HasTenant { get; }
    void SetTenant(long tenantId);
    void ClearTenant();
}
