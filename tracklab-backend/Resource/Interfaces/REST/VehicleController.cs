using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
[Produces("application/json")]
public class VehicleController(IVehicleCommandService commandService, IVehicleQueryService queryService)
    : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleResource>> GetById(long id)
    {
        var vehicle = await queryService.FindByIdAsync(id);
        if (vehicle == null) return NotFound();

        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateVehicleResource resource)
    {
        var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var vehicle = await commandService.Handle(command);

        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(long id, [FromBody] UpdateVehicleResource resource)
    {
        var command = UpdateVehicleInfoCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await commandService.Handle(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await commandService.Handle(new DeleteVehicleCommand(id));
        return NoContent();
    }
}