using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;

namespace Alumware.Tracklab.API.Order.Interfaces.REST;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderCommandService _orderCommandService;
    private readonly IOrderQueryService _orderQueryService;
    private readonly ITrackingEventCommandService _trackingEventCommandService;

    public OrderController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService, ITrackingEventCommandService trackingEventCommandService)
    {
        _orderCommandService = orderCommandService;
        _orderQueryService = orderQueryService;
        _trackingEventCommandService = trackingEventCommandService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResource>>> GetAll(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? status = null,
        [FromQuery] long? customerId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var orderStatus = status != null && Enum.TryParse<Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus>(status, out var parsedStatus) 
            ? parsedStatus 
            : (Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus?)null;

        var query = new GetAllOrdersQuery(pageSize, pageNumber, orderStatus, customerId, fromDate, toDate);
        var orders = await _orderQueryService.Handle(query);
        var resources = OrderResourceFromEntityAssembler.ToResourceFromEntities(orders);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResource>> GetById(long id)
    {
        var query = new GetOrderByIdQuery(id);
        var order = await _orderQueryService.Handle(query);
        
        if (order == null)
            return NotFound();

        var resource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResource>> Create([FromBody] CreateOrderResource resource)
    {
        var command = CreateOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var order = await _orderCommandService.CreateAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, orderResource);
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<OrderResource>> AddOrderItem(long id, [FromBody] AddOrderItemResource resource)
    {
        var command = AddOrderItemCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var order = await _orderCommandService.AddOrderItemAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        
        return Ok(orderResource);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderResource>> UpdateStatus(long id, [FromBody] UpdateOrderStatusResource resource)
    {
        var command = UpdateOrderStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var order = await _orderCommandService.UpdateStatusAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        
        return Ok(orderResource);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var command = new DeleteOrderCommand(id);
        await _orderCommandService.DeleteAsync(command);
        return NoContent();
    }

    [HttpPost("{orderId}/assign-vehicle")]
    public async Task<ActionResult<OrderResource>> AssignVehicle(long orderId, [FromBody] AssignVehicleResource resource)
    {
        var command = AssignVehicleCommandFromResourceAssembler.ToCommandFromResource(orderId, resource);
        var order = await _orderCommandService.AssignVehicleAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return Ok(orderResource);
    }

    [HttpPost("{orderId}/route")]
    public async Task<ActionResult<OrderResource>> SetRoute(long orderId, [FromBody] RouteResource resource)
    {
        var command = SetRouteCommandFromResourceAssembler.ToCommandFromResource(resource with { OrderId = orderId });
        var order = await _orderCommandService.SetRouteAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return Ok(orderResource);
    }

    [HttpPut("{orderId}/route")]
    public async Task<ActionResult<OrderResource>> UpdateRoute(long orderId, [FromBody] RouteResource resource)
    {
        var command = SetRouteCommandFromResourceAssembler.ToCommandFromResource(resource with { OrderId = orderId });
        var order = await _orderCommandService.SetRouteAsync(command);
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return Ok(orderResource);
    }

    [HttpPost("{orderId}/tracking")]
    public async Task<IActionResult> RegisterTrackingEvent(long orderId, [FromBody] TrackingEventResource resource)
    {
        if (orderId != resource.OrderId)
            return BadRequest("El ID de la orden no coincide");
        var command = RegisterTrackingEventCommandFromResourceAssembler.ToCommand(resource);
        var trackingEvent = await _trackingEventCommandService.RegisterTrackingEventAsync(command);
        return Ok(trackingEvent);
    }

    [HttpGet("{orderId}/tracking")]
    public async Task<IActionResult> GetTrackingEvents(long orderId)
    {
        var events = await _trackingEventCommandService.GetTrackingEventsAsync(orderId);
        return Ok(events);
    }
} 