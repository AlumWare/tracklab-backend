using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/employees")]
[Produces("application/json")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeCommandService _commandService;
    private readonly IEmployeeQueryService _queryService;

    public EmployeeController(IEmployeeCommandService commandService, IEmployeeQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeResource>>> GetAll()
    {
        var employees = await _queryService.ListAsync();
        var resources = employees.Select(EmployeeResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResource>> GetById(long id)
    {
        var employee = await _queryService.FindByIdAsync(id);
        if (employee == null) return NotFound();

        var resource = EmployeeResourceFromEntityAssembler.ToResourceFromEntity(employee);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateEmployeeResource resource)
    {
        var command = CreateEmployeeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var created = await _commandService.Handle(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateInfo(long id, [FromBody] UpdateEmployeeResource resource)
    {
        var command = UpdateEmployeeInfoCommandFromResourceAssembler.ToPositionCommand(id, resource);
        await _commandService.Handle(command);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateStatus(long id, [FromQuery] string newStatus)
    {
        var command = UpdateEmployeeInfoCommandFromResourceAssembler.ToStatusCommand(id, newStatus);
        await _commandService.Handle(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await _commandService.Handle(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}