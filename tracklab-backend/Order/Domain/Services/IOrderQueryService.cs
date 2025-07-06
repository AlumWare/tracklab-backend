using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Domain.Services;

public interface IOrderQueryService
{
    Task<IEnumerable<OrderAggregate>> Handle(GetAllOrdersQuery query);
    Task<OrderAggregate?> Handle(GetOrderByIdQuery query);
    Task<IEnumerable<LogisticsCompanyResource>> GetLogisticsCompaniesAsync();
} 