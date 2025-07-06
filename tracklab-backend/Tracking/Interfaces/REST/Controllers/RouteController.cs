using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Controllers;

/// <summary>
/// Controller for managing logistics routes and route planning
/// </summary>
[ApiController]
[Route("api/routes")]
public class RouteController : ControllerBase
{
    private readonly IRouteCommandService _routeCommandService;
    private readonly IRouteQueryService _routeQueryService;
    private readonly IOrderQueryService _orderQueryService;

    public RouteController(
        IRouteCommandService routeCommandService, 
        IRouteQueryService routeQueryService, 
        IOrderQueryService orderQueryService)
    {
        _routeCommandService = routeCommandService;
        _routeQueryService = routeQueryService;
        _orderQueryService = orderQueryService;
    }

    /// <summary>
    /// Gets all routes with optional filtering for logistics planning
    /// </summary>
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

    /// <summary>
    /// Gets a specific route by ID with complete planning details
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RouteResource>> GetById(long id)
    {
        var query = new GetRouteByIdQuery(id);
        var route = await _routeQueryService.Handle(query);
        if (route == null) return NotFound($"Route {id} not found");
        
        var resource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
        return Ok(resource);
    }

    /// <summary>
    /// Creates a new route with complete planning information
    /// Includes vehicle assignment, warehouse sequence, and orders to serve
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RouteResource>> Create([FromBody] CreateRouteResource resource)
    {
        try
        {
            var command = CreateRouteCommandFromResourceAssembler.ToCommandFromResource(resource);
            var route = await _routeCommandService.CreateAsync(command);
            var routeResource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
            
            return CreatedAtAction(nameof(GetById), new { id = route.RouteId }, routeResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Adds a warehouse node to an existing route
    /// Used for dynamic route planning adjustments
    /// </summary>
    [HttpPost("{id}/nodes")]
    public async Task<ActionResult<RouteResource>> AddNode(long id, [FromBody] AddNodeResource resource)
    {
        try
        {
            var command = AddNodeCommandFromResourceAssembler.ToCommandFromResource(resource, id);
            var route = await _routeCommandService.AddNodeAsync(command);
            var routeResource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
            
            return Ok(routeResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Adds an order to an existing route
    /// Used for dynamic order assignment during route execution
    /// </summary>
    [HttpPost("{id}/orders")]
    public async Task<ActionResult<RouteResource>> AddOrder(long id, [FromBody] AddOrderToRouteResource resource)
    {
        try
        {
            // Validate that order exists and is accessible
            var order = await _orderQueryService.Handle(new GetOrderByIdQuery(resource.OrderId));
            if (order == null) 
                return NotFound(new { message = $"Order {resource.OrderId} not found or not accessible" });

            var route = await _routeCommandService.AddOrderAsync(id, order);
            var routeResource = RouteResourceFromEntityAssembler.ToResourceFromEntity(route);
            
            return Ok(routeResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Gets all orders assigned to a specific route
    /// Used for logistics planning and execution
    /// </summary>
    [HttpGet("{id}/orders")]
    public async Task<ActionResult<IEnumerable<object>>> GetRouteOrders(long id)
    {
        var query = new GetRouteByIdQuery(id);
        var route = await _routeQueryService.Handle(query);
        if (route == null) return NotFound($"Route {id} not found");

        var orderInfo = route.Orders.Select(order => new
        {
            OrderId = order.OrderId,
            CustomerId = order.TenantId,
            LogisticsId = order.LogisticsId,
            Status = order.Status.ToString(),
            TotalItems = order.OrderItems?.Sum(item => item.Quantity) ?? 0,
            ShippingAddress = order.ShippingAddress
        });

        return Ok(orderInfo);
    }

    /// <summary>
    /// Gets available products for container creation based on route orders
    /// Used by logistics companies to know what products can be packed in containers
    /// </summary>
    [HttpGet("{id}/available-products")]
    public async Task<ActionResult<Dictionary<long, object>>> GetAvailableProducts(long id)
    {
        try
        {
            var availableProducts = await _routeCommandService.GetAvailableProductsForRouteAsync(id);
            
            var response = availableProducts.ToDictionary(
                kvp => kvp.Key, 
                kvp => (object)new
                {
                    ProductId = kvp.Value.ProductId,
                    AvailableQuantity = kvp.Value.AvailableQuantity,
                    FromOrderId = kvp.Value.FromOrderId
                });

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 