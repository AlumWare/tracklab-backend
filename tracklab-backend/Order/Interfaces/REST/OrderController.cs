using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Queries;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;
using Microsoft.AspNetCore.Mvc;
using IResourceContextServiceOrder = Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL.IResourceContextService;

namespace Alumware.Tracklab.API.Order.Interfaces.REST;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderCommandService _orderCommandService;
    private readonly IOrderQueryService _orderQueryService;
    private readonly ITrackingEventCommandService _trackingEventCommandService;
    private readonly ITrackingContextService _trackingContextService;

    public OrderController(
        IOrderCommandService orderCommandService, 
        IOrderQueryService orderQueryService, 
        ITrackingEventCommandService trackingEventCommandService,
        ITrackingContextService trackingContextService)
    {
        _orderCommandService = orderCommandService;
        _orderQueryService = orderQueryService;
        _trackingEventCommandService = trackingEventCommandService;
        _trackingContextService = trackingContextService;
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

    [HttpGet("{id}/tracking-info")]
    public async Task<ActionResult<OrderTrackingInfoResource>> GetTrackingInfo(long id)
    {
        try
        {
            var trackingInfo = await _trackingContextService.GetOrderTrackingInfoAsync(id);
            var resource = new OrderTrackingInfoResource(
                trackingInfo.OrderId,
                trackingInfo.TotalContainers,
                trackingInfo.DeliveredContainers,
                trackingInfo.InTransitContainers,
                trackingInfo.PendingContainers,
                trackingInfo.OverallStatus,
                trackingInfo.LastUpdated
            );
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderResource>> Create([FromBody] CreateOrderResource resource)
    {
        try
        {
            var command = CreateOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var order = await _orderCommandService.CreateWithProductInfoAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, orderResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<OrderResource>> AddOrderItem(long id, [FromBody] AddOrderItemResource resource)
    {
        try
        {
            var command = AddOrderItemCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var order = await _orderCommandService.AddOrderItemAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return Ok(orderResource);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderResource>> UpdateStatus(long id, [FromBody] UpdateOrderStatusResource resource)
    {
        try
        {
            var command = UpdateOrderStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var order = await _orderCommandService.UpdateStatusAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return Ok(orderResource);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        try
        {
            var command = new DeleteOrderCommand(id);
            await _orderCommandService.DeleteAsync(command);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{orderId}/assign-vehicle")]
    public async Task<ActionResult<OrderResource>> AssignVehicle(long orderId, [FromBody] AssignVehicleResource resource)
    {
        try
        {
            var command = AssignVehicleCommandFromResourceAssembler.ToCommandFromResource(orderId, resource);
            var order = await _orderCommandService.AssignVehicleAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return Ok(orderResource);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{orderId}/route")]
    public async Task<ActionResult<OrderResource>> SetRoute(long orderId, [FromBody] RouteResource resource)
    {
        try
        {
            var command = SetRouteCommandFromResourceAssembler.ToCommandFromResource(resource with { OrderId = orderId });
            var order = await _orderCommandService.SetRouteAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return Ok(orderResource);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{orderId}/route")]
    public async Task<ActionResult<OrderResource>> UpdateRoute(long orderId, [FromBody] RouteResource resource)
    {
        try
        {
            var command = SetRouteCommandFromResourceAssembler.ToCommandFromResource(resource with { OrderId = orderId });
            var order = await _orderCommandService.SetRouteAsync(command);
            var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
            return Ok(orderResource);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("{orderId}/tracking")]
    public async Task<IActionResult> RegisterTrackingEvent(long orderId, [FromBody] TrackingEventResource resource)
    {
        try
        {
            if (orderId != resource.OrderId)
                return BadRequest(new { error = "El ID de la orden no coincide" });
            var command = RegisterTrackingEventCommandFromResourceAssembler.ToCommand(resource);
            var trackingEvent = await _trackingEventCommandService.RegisterTrackingEventAsync(command);
            return Ok(trackingEvent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{orderId}/tracking")]
    public async Task<IActionResult> GetTrackingEvents(long orderId)
    {
        var events = await _trackingEventCommandService.GetTrackingEventsAsync(orderId);
        return Ok(events);
    }

    /// <summary>
    /// Obtener todas las empresas logísticas disponibles
    /// </summary>
    [HttpGet("logistics")]
    [ProducesResponseType(typeof(IEnumerable<LogisticsCompanyResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLogisticsCompanies()
    {
        try
        {
            var logisticsCompanies = await _orderQueryService.GetLogisticsCompaniesAsync();
            return Ok(logisticsCompanies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
        }
    }
} 