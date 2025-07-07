using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<OrderAggregate>
{
    Task<IEnumerable<OrderAggregate>> GetAllAsync(GetAllOrdersQuery query);
    Task<OrderAggregate?> GetByIdAsync(GetOrderByIdQuery query);
    
    /// <summary>
    /// Gets order by ID for ACL validations, bypassing tenant filter
    /// This method should only be used by ACL services for cross-tenant validation
    /// </summary>
    Task<OrderAggregate?> GetByIdForACLAsync(long id);
} 