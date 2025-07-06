using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;
using Microsoft.AspNetCore.Mvc;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductCommandService _productCommandService;
    private readonly IProductQueryService _productQueryService;

    public ProductController(IProductCommandService productCommandService, IProductQueryService productQueryService)
    {
        _productCommandService = productCommandService;
        _productQueryService = productQueryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResource>>> GetAll(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? name = null,
        [FromQuery] string? category = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        var query = new GetAllProductsQuery(pageSize, pageNumber, name, category, minPrice, maxPrice);
        var products = await _productQueryService.Handle(query);
        var resources = ProductResourceFromEntityAssembler.ToResourceFromEntities(products);
        return Ok(resources);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<ProductResource>>> GetAvailableProducts(
        [FromQuery] int? pageSize = null,
        [FromQuery] int? pageNumber = null,
        [FromQuery] string? name = null,
        [FromQuery] string? category = null)
    {
        var query = new GetAvailableProductsQuery(pageSize, pageNumber, name, category);
        var products = await _productQueryService.Handle(query);
        var resources = ProductResourceFromEntityAssembler.ToResourceFromEntities(products);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResource>> GetById(long id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _productQueryService.Handle(query);
        
        if (product == null)
            return NotFound();

        var resource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResource>> Create([FromBody] CreateProductResource resource)
    {
        var command = CreateProductCommandFromResourceAssembler.ToCommandFromResource(resource);
        var product = await _productCommandService.CreateAsync(command);
        var productResource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, productResource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResource>> Update(long id, [FromBody] UpdateProductResource resource)
    {
        var command = UpdateProductInfoCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var product = await _productCommandService.UpdateAsync(command);
        var productResource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        
        return Ok(productResource);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var command = new DeleteProductCommand(id);
        await _productCommandService.DeleteAsync(command);
        return NoContent();
    }
} 