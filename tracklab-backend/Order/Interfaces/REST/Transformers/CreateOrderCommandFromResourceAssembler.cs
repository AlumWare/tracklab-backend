using System.Linq;
using System.Collections.Generic;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Domain.Repositories;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class CreateOrderCommandFromResourceAssembler
{
    public static async Task<CreateOrderWithProductInfoCommand> ToCommandFromResourceAsync(
        CreateOrderResource resource, 
        IProductRepository productRepository)
    {
        var items = new List<AddOrderItemWithPriceCommand>();
        
        foreach (var item in resource.Items)
        {
            // Obtener el producto para validar stock y obtener precio
            var product = await productRepository.FindByIdAsync(item.ProductId);
            if (product == null)
                throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado.");
            
            if (product.Stock < item.Quantity)
                throw new ArgumentException($"Stock insuficiente para el producto {product.Name}. Disponible: {product.Stock}, Solicitado: {item.Quantity}");
            
            items.Add(new AddOrderItemWithPriceCommand(
                item.ProductId,
                item.Quantity,
                product.Price.Amount,
                product.Price.Currency
            ));
        }
        
        return new CreateOrderWithProductInfoCommand(
            resource.CustomerId,
            resource.LogisticsId,
            resource.ShippingAddress,
            items
        );
    }
} 