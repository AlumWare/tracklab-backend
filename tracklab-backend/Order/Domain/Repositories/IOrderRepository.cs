using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<OrderAggregate>
{
    Task<IEnumerable<OrderAggregate>> GetAllAsync(GetAllOrdersQuery query);
    Task<OrderAggregate?> GetByIdAsync(GetOrderByIdQuery query);
} 