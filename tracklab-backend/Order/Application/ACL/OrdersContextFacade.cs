using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Interfaces.ACL;

namespace Alumware.Tracklab.API.Order.Application.ACL;

public class OrdersContextFacade(
    IOrderCommandService orderCommandService,
    IOrderQueryService orderQueryService
    ) : IOrdersContextFacade
{
    public async Task<long> CreateOrder(
        long customerId,
        long logisticsId,
        string shippingAddress)
    {
        var createOrderCommand = new CreateOrderCommand(customerId, logisticsId, shippingAddress);
        var order = await orderCommandService.CreateAsync(createOrderCommand);
        return order?.OrderId ?? 0;
    }
}