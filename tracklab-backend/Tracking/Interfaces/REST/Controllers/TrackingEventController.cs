using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Controllers;

[ApiController]
[Route("api/tracking-events")]
public class TrackingEventController : ControllerBase
{
    private readonly ITrackingEventCommandService _trackingEventCommandService;
    private readonly ITrackingEventQueryService _trackingEventQueryService;

    public TrackingEventController(ITrackingEventCommandService trackingEventCommandService, ITrackingEventQueryService trackingEventQueryService)
    {
        _trackingEventCommandService = trackingEventCommandService;
        _trackingEventQueryService = trackingEventQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrackingEventResource>>> GetAll(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] long? containerId = null,
        [FromQuery] long? warehouseId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var query = new GetAllTrackingEventsQuery(pageSize, pageNumber, containerId, warehouseId, fromDate, toDate);
        var events = await _trackingEventQueryService.Handle(query);
        var resources = TrackingEventResourceFromEntityAssembler.ToResourceFromEntities(events);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrackingEventResource>> GetById(long id)
    {
        var query = new GetTrackingEventByIdQuery(id);
        var trackingEvent = await _trackingEventQueryService.Handle(query);
        if (trackingEvent == null) return NotFound();
        var resource = TrackingEventResourceFromEntityAssembler.ToResourceFromEntity(trackingEvent);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<TrackingEventResource>> Create([FromBody] CreateTrackingEventCommand command)
    {
        var trackingEvent = await _trackingEventCommandService.CreateAsync(command);
        var resource = TrackingEventResourceFromEntityAssembler.ToResourceFromEntity(trackingEvent);
        return CreatedAtAction(nameof(GetById), new { id = trackingEvent.EventId }, resource);
    }
} 