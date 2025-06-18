using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IProductQueryService
{
    Task<IEnumerable<Product>> Handle(GetAllProductsQuery query);
    Task<Product?> Handle(GetProductByIdQuery query);
} 