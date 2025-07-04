using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Application.Internal.QueryServices;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _orderRepository;

    public OrderQueryService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderAggregate>> Handle(GetAllOrdersQuery query)
    {
        return await _orderRepository.GetAllAsync(query);
    }

    public async Task<OrderAggregate?> Handle(GetOrderByIdQuery query)
    {
        return await _orderRepository.FindByIdAsync(query.Id);
    }
} 