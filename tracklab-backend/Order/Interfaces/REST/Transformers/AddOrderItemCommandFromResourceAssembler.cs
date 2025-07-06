using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Domain.Repositories;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class AddOrderItemCommandFromResourceAssembler
{
    public static async Task<AddOrderItemCommand> ToCommandFromResourceAsync(
        long orderId, 
        AddOrderItemResource resource,
        IProductRepository productRepository)
    {
        // Obtener el producto para validar stock y obtener precio
        var product = await productRepository.FindByIdAsync(resource.ProductId);
        if (product == null)
            throw new ArgumentException($"Producto con ID {resource.ProductId} no encontrado.");
        
        if (product.Stock < resource.Quantity)
            throw new ArgumentException($"Stock insuficiente para el producto {product.Name}. Disponible: {product.Stock}, Solicitado: {resource.Quantity}");
        
        return new AddOrderItemCommand(
            orderId,
            resource.ProductId,
            resource.Quantity,
            product.Price.Amount,
            product.Price.Currency
        );
    }
} 