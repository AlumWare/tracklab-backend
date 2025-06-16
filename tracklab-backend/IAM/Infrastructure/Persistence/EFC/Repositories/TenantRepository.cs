using Microsoft.EntityFrameworkCore;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace TrackLab.IAM.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Repository implementation for Tenant aggregate
/// </summary>
public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext context) : base(context) { }

    public async Task<Tenant?> FindByRucAsync(string ruc)
    {
        return await Context.Set<Tenant>()
            .FirstOrDefaultAsync(t => t.Ruc == ruc);
    }

    public async Task<IEnumerable<Tenant>> FindByCountryAsync(string country)
    {
        return await Context.Set<Tenant>()
            .Where(t => t.Country == country)
            .ToListAsync();
    }
} 