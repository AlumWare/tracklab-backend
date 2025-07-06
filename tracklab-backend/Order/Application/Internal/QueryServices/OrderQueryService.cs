using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.IAM.Domain.Model.ValueObjects;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Application.Internal.QueryServices;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITenantRepository _tenantRepository;

    public OrderQueryService(IOrderRepository orderRepository, ITenantRepository tenantRepository)
    {
        _orderRepository = orderRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task<IEnumerable<OrderAggregate>> Handle(GetAllOrdersQuery query)
    {
        return await _orderRepository.GetAllAsync(query);
    }

    public async Task<OrderAggregate?> Handle(GetOrderByIdQuery query)
    {
        return await _orderRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<OrderResource>> GetAllAsync(GetAllOrdersQuery query)
    {
        var orders = await _orderRepository.GetAllAsync(query);
        return orders.Select(OrderResourceFromEntityAssembler.ToResourceFromEntity);
    }

    public async Task<OrderResource?> GetByIdAsync(GetOrderByIdQuery query)
    {
        var order = await _orderRepository.GetByIdAsync(query);
        return order != null ? OrderResourceFromEntityAssembler.ToResourceFromEntity(order) : null;
    }

    public async Task<IEnumerable<LogisticsCompanyResource>> GetLogisticsCompaniesAsync()
    {
        // Obtener todos los tenants y filtrar solo los de tipo LOGISTIC
        var allTenants = await _tenantRepository.ListAsync();
        var logisticsTenants = allTenants.Where(t => t.IsLogistic() && t.Active);
        
        return logisticsTenants.Select(tenant => new LogisticsCompanyResource(
            tenant.Id,
            tenant.LegalName,
            tenant.CommercialName,
            tenant.Address,
            tenant.City,
            tenant.Country,
            tenant.GetEmail(),
            tenant.GetPhone(),
            tenant.Website
        ));
    }
} 