using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The order repository
 * </summary>
 * <remarks>
 *     This repository is used to manage orders with tenant awareness.
 *     Orders can be accessed by both the customer (TenantId) and the logistics company (LogisticsId)
 * </remarks>
 */
public class OrderRepository : BaseRepository<OrderAggregate>, IOrderRepository
{
    private readonly ITenantContext _tenantContext;

    public OrderRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied.
    /// Orders can be accessed by both the customer (TenantId) and the logistics company (LogisticsId)
    /// </summary>
    private IQueryable<OrderAggregate> GetTenantFilteredQuery()
    {
        var query = Context.Set<OrderAggregate>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            // Permitir acceso si el tenant actual es el cliente O la empresa logística
            query = query.Where(o => o.TenantId == currentTenantId || o.LogisticsId == currentTenantId);
        }
        
        return query;
    }

    public async Task<IEnumerable<OrderAggregate>> GetAllAsync(GetAllOrdersQuery query)
    {
        var ordersQuery = GetTenantFilteredQuery();

        // Aplicar filtros
        if (query.Status.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.Status == query.Status.Value);
        }

        if (query.CustomerId.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.TenantId == query.CustomerId.Value);
        }

        if (query.FromDate.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderDate >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderDate <= query.ToDate.Value);
        }

        // Aplicar paginación
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            ordersQuery = ordersQuery
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }

        // Incluir OrderItems al final
        ordersQuery = ordersQuery.Include(o => o.OrderItems);

        return await ordersQuery.ToListAsync();
    }

    public async Task<OrderAggregate?> GetByIdAsync(GetOrderByIdQuery query)
    {
        return await GetTenantFilteredQuery()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == query.Id);
    }
    
    public new async Task<OrderAggregate?> FindByIdAsync(long id)
    {
        // IMPORTANTE: Este método bypassa el filtro de tenant, solo debe usarse en contextos internos
        // donde ya se ha validado el acceso (como en el ACL)
        return await Context.Set<OrderAggregate>()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    /// <summary>
    /// Gets order by ID for ACL validations, bypassing tenant filter
    /// This method should only be used by ACL services for cross-tenant validation
    /// </summary>
    public async Task<OrderAggregate?> GetByIdForACLAsync(long id)
    {
        return await Context.Set<OrderAggregate>()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }
} 