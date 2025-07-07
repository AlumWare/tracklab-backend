using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
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
        return query;
    }

    public async Task<IEnumerable<Container>> GetAllAsync(GetAllContainersQuery query)
    {
        var containersQuery = GetTenantFilteredQuery();
        
        if (query.OrderId.HasValue)
        {
            var orderIdValueObject = new OrderId(query.OrderId.Value);
            containersQuery = containersQuery.Where(c => c.OrderId == orderIdValueObject);
        }
        
        if (query.WarehouseId.HasValue)
        {
            var warehouseIdValueObject = new WarehouseId(query.WarehouseId.Value);
            containersQuery = containersQuery.Where(c => c.WarehouseId == warehouseIdValueObject);
        }
        
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
            containersQuery = containersQuery.Skip((query.PageNumber.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value);
        
        return await containersQuery.ToListAsync();
    }

    public async Task<Container?> GetByIdAsync(GetContainerByIdQuery query)
    {
        return await GetTenantFilteredQuery().FirstOrDefaultAsync(c => c.ContainerId == query.Id);
    }

    public async Task<IEnumerable<Container>> GetByOrderIdAsync(long orderId)
    {
        var orderIdValueObject = new OrderId(orderId);
        return await GetTenantFilteredQuery()
            .Where(c => c.OrderId == orderIdValueObject)
            .ToListAsync();
    }
} 