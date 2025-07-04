using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The product repository
 * </summary>
 * <remarks>
 *     This repository is used to manage products with tenant awareness
 * </remarks>
 */
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly ITenantContext _tenantContext;

    public ProductRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<Product> GetTenantFilteredQuery()
    {
        var query = Context.Set<Product>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(p => p.TenantId == currentTenantId);
        }
        
        return query;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(GetAllProductsQuery query)
    {
        var productsQuery = GetTenantFilteredQuery();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(query.Name));
        }

        if (query.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price.Amount >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price.Amount <= query.MaxPrice.Value);
        }

        // Aplicar paginación
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            productsQuery = productsQuery
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }

        return await productsQuery.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(GetProductByIdQuery query)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(p => p.Id == query.Id);
    }

    public new async Task<Product?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
} 