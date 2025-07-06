using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Repositories;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class ProductQueryService : IProductQueryService
{
    private readonly IProductRepository _productRepository;

    public ProductQueryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery query)
    {
        return await _productRepository.GetAllAsync(query);
    }

    public async Task<Product?> Handle(GetProductByIdQuery query)
    {
        return await _productRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Product>> Handle(GetAvailableProductsQuery query)
    {
        return await _productRepository.GetAvailableAsync(query);
    }
} 