using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Order.Infrastructure.Persistence.EFC.Repositories;

public class TrackingEventRepository : ITrackingEventRepository
{
    private readonly AppDbContext _context;
    public TrackingEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TrackingEvent trackingEvent)
    {
        await _context.OrderTrackingEvents.AddAsync(trackingEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TrackingEvent>> GetByOrderIdAsync(long orderId)
    {
        return await _context.OrderTrackingEvents
            .Where(e => e.OrderId == orderId)
            .OrderBy(e => e.Sequence)
            .ToListAsync();
    }

    public async Task<TrackingEvent?> GetLastByOrderIdAsync(long orderId)
    {
        return await _context.OrderTrackingEvents
            .Where(e => e.OrderId == orderId)
            .OrderByDescending(e => e.Sequence)
            .FirstOrDefaultAsync();
    }
} 