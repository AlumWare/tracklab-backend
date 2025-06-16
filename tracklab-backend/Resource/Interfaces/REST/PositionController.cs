using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Controllers;
[ApiController]
[Route("api/v1/positions")]
[Produces("application/json")]
public class PositionController : ControllerBase
{
    private readonly IPositionCommandService _commandService;
    private readonly IPositionQueryService _queryService;

    public PositionController(IPositionCommandService commandService, IPositionQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionResource>>> GetAll(
        [FromQuery] int? pageSize = null, 
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? department = null,
        [FromQuery] bool? isActive = null)
    {
        var query = new GetAllPositionsQuery(pageSize, pageNumber, department, isActive);
        var positions = await _queryService.Handle(query);
        var resources = positions.Select(PositionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PositionResource>> GetById(long id)
    {
        var position = await _queryService.Handle(new GetPositionByIdQuery(id));
        if (position is null) return NotFound();

        var resource = PositionResourceFromEntityAssembler.ToResourceFromEntity(position);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePositionResource resource)
    {
        var command = CreatePositionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var created = await _commandService.Handle(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(long id, [FromBody] UpdatePositionResource resource)
    {
        var command = UpdatePositionNameCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await _commandService.Handle(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await _commandService.Handle(new DeletePositionCommand(id));
        return NoContent();
    }
}