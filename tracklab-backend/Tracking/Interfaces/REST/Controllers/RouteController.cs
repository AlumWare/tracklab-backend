using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Order.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Controllers;

[ApiController]
[Route("api/routes")]
public class RouteController : ControllerBase
{
    private readonly IRouteCommandService _routeCommandService;
    private readonly IRouteQueryService _routeQueryService;
    private readonly IOrderQueryService _orderQueryService;

    public RouteController(IRouteCommandService routeCommandService, IRouteQueryService routeQueryService, IOrderQueryService orderQueryService)
    {
        _routeCommandService = routeCommandService;
        _routeQueryService = routeQueryService;
        _orderQueryService = orderQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteResource>>> GetAll(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] long? vehicleId = null,
        [FromQuery] long? orderId = null)
    {
        var query = new GetAllRoutesQuery(pageSize, pageNumber, vehicleId, orderId);
        var routes = await _routeQueryService.Handle(query);
        var resources = RouteResourceFromEntityAssembler.ToResourceFromEntities(routes);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RouteResource>> GetById(long id)
    {
        var query = new GetRouteByIdQuery(id);
        var route = await _routeQueryService.Handle(query);
        if (route == null) return NotFound();
        var resource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<RouteResource>> Create([FromBody] CreateRouteCommand command)
    {
        var route = await _routeCommandService.CreateAsync(command);
        var resource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
        return CreatedAtAction(nameof(GetById), new { id = route.RouteId }, resource);
    }

    [HttpPost("{id}/nodes")]
    public async Task<ActionResult<RouteResource>> AddNode(long id, [FromBody] AddNodeCommand command)
    {
        var route = await _routeCommandService.AddNodeAsync(command with { RouteId = id });
        var resource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
        return Ok(resource);
    }

    [HttpPost("{id}/orders")]
    public async Task<ActionResult<RouteResource>> AddOrder(long id, [FromBody] AddOrderToRouteCommand command)
    {
        var order = await _orderQueryService.Handle(new Alumware.Tracklab.API.Order.Domain.Model.Queries.GetOrderByIdQuery(command.OrderId));
        if (order == null) return NotFound($"Order {command.OrderId} not found");
        var route = await _routeCommandService.AddOrderAsync(id, order);
        var resource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
        return Ok(resource);
    }
} 