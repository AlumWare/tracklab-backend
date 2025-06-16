using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.Shared.Domain.Repositories;

namespace TrackLab.IAM.Domain.Repositories;

/// <summary>
/// Repository interface for Tenant aggregate
/// </summary>
public interface ITenantRepository : IBaseRepository<Tenant>
{
    /// <summary>
    /// Find tenant by RUC
    /// </summary>
    Task<Tenant?> FindByRucAsync(string ruc);
    
    /// <summary>
    /// Find tenants by country
    /// </summary>
    Task<IEnumerable<Tenant>> FindByCountryAsync(string country);
} 