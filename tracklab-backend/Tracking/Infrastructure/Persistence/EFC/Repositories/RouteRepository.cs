using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;

public class RouteRepository : BaseRepository<RouteAggregate>, IRouteRepository
{
    private readonly ITenantContext _tenantContext;

    public RouteRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    private IQueryable<RouteAggregate> GetTenantFilteredQuery()
    {
        var query = Context.Set<RouteAggregate>().AsQueryable();
        // Aquí podrías filtrar por tenant si corresponde
        return query;
    }

    public async Task<IEnumerable<RouteAggregate>> GetAllAsync(GetAllRoutesQuery query)
    {
        var routesQuery = GetTenantFilteredQuery();
        if (query.VehicleId.HasValue)
            routesQuery = routesQuery.Where(r => r.VehicleId.Value == query.VehicleId.Value);
        if (query.OrderId.HasValue)
            routesQuery = routesQuery.Where(r => r.Orders.Any(o => o.OrderId == query.OrderId.Value));
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
            routesQuery = routesQuery.Skip((query.PageNumber.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value);
        return await routesQuery.ToListAsync();
    }

    public async Task<RouteAggregate?> GetByIdAsync(GetRouteByIdQuery query)
    {
        return await GetTenantFilteredQuery().FirstOrDefaultAsync(r => r.RouteId == query.Id);
    }
} 