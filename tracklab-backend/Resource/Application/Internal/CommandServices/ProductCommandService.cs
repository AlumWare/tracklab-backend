using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class ProductCommandService : IProductCommandService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public ProductCommandService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Product> CreateAsync(CreateProductCommand command)
    {
        var product = new Product(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            product.SetTenantId(new TrackLab.Shared.Domain.ValueObjects.TenantId(_tenantContext.CurrentTenantId!.Value));
        }
        
        await _productRepository.AddAsync(product);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return product;
    }

    public async Task<Product> UpdateAsync(UpdateProductInfoCommand command)
    {
        var product = await _productRepository.FindByIdAsync(command.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Producto {command.ProductId} no encontrado.");

        product.UpdateInfo(command);
        _productRepository.Update(product);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return product;
    }

    public async Task DeleteAsync(DeleteProductCommand command)
    {
        var product = await _productRepository.FindByIdAsync(command.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Producto {command.ProductId} no encontrado.");

        _productRepository.Remove(product);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }
} 