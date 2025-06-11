using Microsoft.AspNetCore.Mvc;
using TrackLab.Resources.Domain.Model.Commands;
using TrackLab.Resources.Domain.Model.Queries;
using TrackLab.Resources.Domain.Services;
using TrackLab.Resources.Interfaces.REST.Resources;
using TrackLab.Resources.Interfaces.REST.Transformers;
using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Interfaces.REST.Controllers;

/// <summary>
/// Controller for warehouse operations
/// </summary>
[ApiController]
[Route("api/v1/tenants/{tenantId}/[controller]")]
public class WarehouseController(
    IWarehouseCommandService warehouseCommandService,
    IWarehouseQueryService warehouseQueryService) : ControllerBase
{
    /// <summary>
    /// Gets all warehouses for a tenant
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>List of warehouses</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WarehouseResource>>> GetAllWarehouses(int tenantId)
    {
        var query = new GetAllWarehousesQuery(new TenantId(tenantId));
        var warehouses = await warehouseQueryService.Handle(query);
        var resources = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouses);
        return Ok(resources);
    }

    /// <summary>
    /// Gets warehouses by type for a tenant
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="type">Warehouse type</param>
    /// <returns>List of warehouses</returns>
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<WarehouseResource>>> GetWarehousesByType(int tenantId, string type)
    {
        if (!Enum.TryParse<TrackLab.Resources.Domain.Model.ValueObjects.WarehouseType>(type, true, out var warehouseType))
        {
            return BadRequest($"Invalid warehouse type: {type}");
        }

        var query = new GetWarehousesByTypeQuery(new TenantId(tenantId), warehouseType);
        var warehouses = await warehouseQueryService.Handle(query);
        var resources = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouses);
        return Ok(resources);
    }

    /// <summary>
    /// Gets a warehouse by id
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="id">Warehouse identifier</param>
    /// <returns>Warehouse</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<WarehouseResource>> GetWarehouseById(int tenantId, int id)
    {
        var query = new GetWarehouseByIdQuery(id, new TenantId(tenantId));
        var warehouse = await warehouseQueryService.Handle(query);
        
        if (warehouse == null)
        {
            return NotFound($"Warehouse with id {id} not found for tenant {tenantId}");
        }

        var resource = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse);
        return Ok(resource);
    }

    /// <summary>
    /// Creates a new warehouse
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="resource">Create warehouse resource</param>
    /// <returns>Created warehouse</returns>
    [HttpPost]
    public async Task<ActionResult<WarehouseResource>> CreateWarehouse(int tenantId, [FromBody] CreateWarehouseResource resource)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateWarehouseCommandFromResourceAssembler.ToCommandFromResource(resource, new TenantId(tenantId));
        var warehouse = await warehouseCommandService.Handle(command);
        var warehouseResource = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse);
        
        return CreatedAtAction(
            nameof(GetWarehouseById),
            new { tenantId, id = warehouse.Id },
            warehouseResource);
    }

    /// <summary>
    /// Updates an existing warehouse
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="id">Warehouse identifier</param>
    /// <param name="resource">Update warehouse resource</param>
    /// <returns>Updated warehouse</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<WarehouseResource>> UpdateWarehouse(int tenantId, int id, [FromBody] UpdateWarehouseResource resource)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = UpdateWarehouseCommandFromResourceAssembler.ToCommandFromResource(id, resource, new TenantId(tenantId));
            var warehouse = await warehouseCommandService.Handle(command);
            var warehouseResource = WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse);
            return Ok(warehouseResource);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a warehouse
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="id">Warehouse identifier</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWarehouse(int tenantId, int id)
    {
        try
        {
            var command = new DeleteWarehouseCommand(id, new TenantId(tenantId));
            await warehouseCommandService.Handle(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 