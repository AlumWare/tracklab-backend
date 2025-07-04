using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;

public class TrackingEventRepository : BaseRepository<TrackingEvent>, ITrackingEventRepository
{
    private readonly ITenantContext _tenantContext;

    public TrackingEventRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    private IQueryable<TrackingEvent> GetTenantFilteredQuery()
    {
        var query = Context.Set<TrackingEvent>().AsQueryable();
        // Aquí podrías filtrar por tenant si corresponde
        return query;
    }

    public async Task<IEnumerable<TrackingEvent>> GetAllAsync(GetAllTrackingEventsQuery query)
    {
        var eventsQuery = GetTenantFilteredQuery();
        if (query.ContainerId.HasValue)
            eventsQuery = eventsQuery.Where(e => e.ContainerId == query.ContainerId.Value);
        if (query.WarehouseId.HasValue)
            eventsQuery = eventsQuery.Where(e => e.WarehouseId.Value == query.WarehouseId.Value);
        if (query.FromDate.HasValue)
            eventsQuery = eventsQuery.Where(e => e.EventTime >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            eventsQuery = eventsQuery.Where(e => e.EventTime <= query.ToDate.Value);
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
            eventsQuery = eventsQuery.Skip((query.PageNumber.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value);
        return await eventsQuery.ToListAsync();
    }

    public async Task<TrackingEvent?> GetByIdAsync(GetTrackingEventByIdQuery query)
    {
        return await GetTenantFilteredQuery().FirstOrDefaultAsync(e => e.EventId == query.Id);
    }
} 