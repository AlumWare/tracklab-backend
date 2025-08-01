﻿using Microsoft.AspNetCore.Mvc;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
[Produces("application/json")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleCommandService _commandService;
    private readonly IVehicleQueryService _queryService;

    public VehicleController(IVehicleCommandService commandService, IVehicleQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleResource>>> GetAll(
        [FromQuery] int? pageSize = null, 
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? status = null,
        [FromQuery] string? type = null,
        [FromQuery] string? licensePlate = null)
    {
        var query = new GetAllVehiclesQuery(pageSize, pageNumber, status, type, licensePlate);
        var vehicles = await _queryService.Handle(query);
        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleResource>> GetById(long id)
    {
        var vehicle = await _queryService.Handle(new GetVehicleByIdQuery(id));
        if (vehicle == null) return NotFound();
        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<VehicleResource>> Create([FromForm] CreateVehicleResource resource)
    {
        var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var vehicle = await _commandService.Handle(command);
        var vehicleResource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicleResource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(long id, [FromBody] UpdateVehicleResource resource)
    {
        var command = UpdateVehicleInfoCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await _commandService.Handle(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        await _commandService.Handle(new DeleteVehicleCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/images")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> AddImage(long id, [FromForm] AddVehicleImageResource resource)
    {
        var command = AddVehicleImageCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await _commandService.Handle(command);
        return Ok(new { message = "Imagen agregada exitosamente" });
    }

    [HttpDelete("{id}/images/{publicId}")]
    public async Task<ActionResult> RemoveImage(long id, string publicId)
    {
        var command = new RemoveVehicleImageCommand(id, publicId);
        await _commandService.Handle(command);
        return Ok(new { message = "Imagen removida exitosamente" });
    }
}