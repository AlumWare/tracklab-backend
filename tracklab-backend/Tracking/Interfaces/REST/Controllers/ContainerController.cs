using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Controllers;

[ApiController]
[Route("api/containers")]
public class ContainerController : ControllerBase
{
    private readonly IContainerCommandService _containerCommandService;
    private readonly IContainerQueryService _containerQueryService;

    public ContainerController(IContainerCommandService containerCommandService, IContainerQueryService containerQueryService)
    {
        _containerCommandService = containerCommandService;
        _containerQueryService = containerQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContainerResource>>> GetAll(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] long? orderId = null,
        [FromQuery] long? warehouseId = null)
    {
        var query = new GetAllContainersQuery(pageSize, pageNumber, orderId, warehouseId);
        var containers = await _containerQueryService.Handle(query);
        var resources = ContainerResourceFromEntityAssembler.ToResourceFromEntities(containers);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContainerResource>> GetById(long id)
    {
        var query = new GetContainerByIdQuery(id);
        var container = await _containerQueryService.Handle(query);
        if (container == null) return NotFound();
        var resource = ContainerResourceFromEntityAssembler.ToResourceFromEntity(container);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<ContainerResource>> Create([FromBody] CreateContainerResource resource)
    {
        try
        {
            var command = CreateContainerCommandFromResourceAssembler.ToCommandFromResource(resource);
            var container = await _containerCommandService.CreateAsync(command);
            var containerResource = ContainerResourceFromEntityAssembler.ToResourceFromEntity(container);
            return CreatedAtAction(nameof(GetById), new { id = container.ContainerId }, containerResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "Error interno del servidor al crear el contenedor." });
        }
    }

    [HttpPut("{id}/nodes")]
    public async Task<ActionResult<ContainerResource>> UpdateCurrentNode(long id, [FromBody] UpdateContainerNodeCommand command)
    {
        var container = await _containerCommandService.UpdateCurrentNodeAsync(command with { ContainerId = id });
        var resource = ContainerResourceFromEntityAssembler.ToResourceFromEntity(container);
        return Ok(resource);
    }
} 