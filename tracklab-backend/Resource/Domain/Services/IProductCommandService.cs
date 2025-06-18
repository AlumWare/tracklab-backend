using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IProductCommandService
{
    Task<Product> CreateAsync(CreateProductCommand command);
    Task<Product> UpdateAsync(UpdateProductInfoCommand command);
    Task DeleteAsync(DeleteProductCommand command);
} 