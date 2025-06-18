using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public class Product
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Price Price { get; private set; } = null!;

    // Constructor requerido por EF Core
    public Product() { }

    // Constructor de dominio
    public Product(CreateProductCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        Price = new Price(command.PriceAmount, command.PriceCurrency);
        TenantId = new TenantId(1);
    }

    public void UpdateInfo(UpdateProductInfoCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        Price = new Price(command.PriceAmount, command.PriceCurrency);
    }

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
} 