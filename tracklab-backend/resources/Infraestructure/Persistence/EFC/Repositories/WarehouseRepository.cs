using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace TrackLab.Resources.Infraestructure.Persistence.EFC.Repositories;

public class WarehouseRepository(AppDbContext context) : BaseRepository<Warehouse>(context), IWarehouseRepository
{
    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await FindByIdAsync(id);
    }
    
    public async Task<IEnumerable<Warehouse>> GetByTenantIdAsync(TenantId tenantId)
    {
        return await Context.Set<Warehouse>()
            .Where(w => w.TenantIdValue == tenantId.Value)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Warehouse>> GetByTenantIdAndTypeAsync(TenantId tenantId, WarehouseType type)
    {
        return await Context.Set<Warehouse>()
            .Where(w => w.TenantIdValue == tenantId.Value && w.Type == type)
            .ToListAsync();
    }
    
    public async Task<Warehouse?> GetByIdAndTenantIdAsync(int id, TenantId tenantId)
    {
        return await Context.Set<Warehouse>()
            .FirstOrDefaultAsync(w => w.Id == id && w.TenantIdValue == tenantId.Value);
    }
}