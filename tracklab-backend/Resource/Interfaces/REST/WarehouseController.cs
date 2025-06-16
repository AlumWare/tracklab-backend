using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;

[ApiController]
[Route("api/v1/warehouses")]
[Produces("application/json")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseCommandService _commandService;
    private readonly IWarehouseQueryService _queryService;

    public WarehouseController(IWarehouseCommandService commandService, IWarehouseQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WarehouseResource>>> GetAll(
        [FromQuery] int? pageSize = null, 
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? location = null,
        [FromQuery] string? type = null,
        [FromQuery] bool? isActive = null)
    {
        var query = new GetAllWarehousesQuery(pageSize, pageNumber, location, type, isActive);
        var warehouses = await _queryService.Handle(query);
        var resources = warehouses.Select(WarehouseResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WarehouseResource>> GetById(long id)
    {
        var warehouse = await _queryService.Handle(new GetWarehouseByIdQuery(id));
        if (warehouse is null) return NotFound();

        var resource = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateWarehouseResource resource)
    {
        var command = CreateWarehouseCommandFromResourceAssembler.ToCommandFromResource(resource);
        var created = await _commandService.Handle(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(long id, [FromBody] UpdateWarehouseResource resource)
    {
        var command = UpdateWarehouseInfoCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await _commandService.Handle(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await _commandService.Handle(new DeleteWarehouseCommand(id));
        return NoContent();
    }
}