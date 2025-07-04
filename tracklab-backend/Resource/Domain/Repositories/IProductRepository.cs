using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Resource.Domain.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IEnumerable<Product>> GetAllAsync(GetAllProductsQuery query);
    Task<Product?> GetByIdAsync(GetProductByIdQuery query);
} 