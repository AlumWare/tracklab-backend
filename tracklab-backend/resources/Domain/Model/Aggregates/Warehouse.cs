using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;
using TrackLab.Resources.Domain.Model.Commands;

namespace TrackLab.Domain.Model.Aggregates;

/// <summary>
/// Warehouse Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa la raíz del agregado Warehouse.
/// Contiene las propiedades y métodos para gestionar la información del almacén.
/// </remarks>
public partial class Warehouse
{
    public int Id { get; private set; }
    public TenantId TenantId { get; private set; }
    public string Name { get; private set; }
    public WarehouseType Type { get; private set; }
    public StreetAddress Address { get; private set; }
    public Coordinates Location { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    
    public string FullAddress => Address.FullAddress;

    // Constructor para EF Core
    private Warehouse()
    {
        TenantId = new TenantId(0);
        Name = string.Empty;
        Type = WarehouseType.Client;
        Address = new StreetAddress(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        Location = new Coordinates(0, 0);
        Phone = string.Empty;
        Email = string.Empty;
    }
    
    public Warehouse(TenantId tenantId, string name, WarehouseType type, 
                    string street, string city, string state, string postalCode, string country,
                    double latitude, double longitude, string phone, string email)
    {
        TenantId = tenantId;
        Name = name;
        Type = type;
        Address = new StreetAddress(street, city, state, postalCode, country);
        Location = new Coordinates(latitude, longitude);
        Phone = phone;
        Email = email;
    }

    public Warehouse(CreateWarehouseCommand command)
    {
        TenantId = command.TenantId;
        Name = command.Name;
        Type = command.Type;
        Address = new StreetAddress(command.Street, command.City, command.State, command.PostalCode, command.Country);
        Location = new Coordinates(command.Latitude, command.Longitude);
        Phone = command.Phone;
        Email = command.Email;
    }

    public void UpdateWarehouse(string name, WarehouseType type,
                               string street, string city, string state, string postalCode, string country,
                               double latitude, double longitude, string phone, string email)
    {
        Name = name;
        Type = type;
        Address = new StreetAddress(street, city, state, postalCode, country);
        Location = new Coordinates(latitude, longitude);
        Phone = phone;
        Email = email;
    }
}
