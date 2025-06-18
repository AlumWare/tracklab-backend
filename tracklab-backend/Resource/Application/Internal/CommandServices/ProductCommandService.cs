using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Repositories;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class ProductCommandService : IProductCommandService
{
    private readonly IProductRepository _productRepository;

    public ProductCommandService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> CreateAsync(CreateProductCommand command)
    {
        var product = new Product(command);
        await _productRepository.AddAsync(product);
        return product;
    }

    public async Task<Product> UpdateAsync(UpdateProductInfoCommand command)
    {
        var product = await _productRepository.FindByIdAsync(command.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Producto {command.ProductId} no encontrado.");

        product.UpdateInfo(command);
        _productRepository.Update(product);
        return product;
    }

    public async Task DeleteAsync(DeleteProductCommand command)
    {
        var product = await _productRepository.FindByIdAsync(command.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Producto {command.ProductId} no encontrado.");

        _productRepository.Remove(product);
    }
} 