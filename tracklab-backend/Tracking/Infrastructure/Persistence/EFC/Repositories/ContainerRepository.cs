using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;

public class ContainerRepository : BaseRepository<Container>, IContainerRepository
{
    private readonly ITenantContext _tenantContext;

    public ContainerRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    private IQueryable<Container> GetTenantFilteredQuery()
    {
        var query = Context.Set<Container>().AsQueryable();
        // Aquí podrías filtrar por tenant si corresponde
        return query;
    }

    public async Task<IEnumerable<Container>> GetAllAsync(GetAllContainersQuery query)
    {
        var containersQuery = GetTenantFilteredQuery();
        if (query.OrderId.HasValue)
            containersQuery = containersQuery.Where(c => c.OrderId.Value == query.OrderId.Value);
        if (query.WarehouseId.HasValue)
            containersQuery = containersQuery.Where(c => c.WarehouseId.Value == query.WarehouseId.Value);
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
            containersQuery = containersQuery.Skip((query.PageNumber.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value);
        return await containersQuery.ToListAsync();
    }

    public async Task<Container?> GetByIdAsync(GetContainerByIdQuery query)
    {
        return await GetTenantFilteredQuery().FirstOrDefaultAsync(c => c.ContainerId == query.Id);
    }
} 