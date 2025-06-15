using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;
using TrackLab.Resources.Domain.Model.Commands;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    // Primitive properties for Entity Framework
    public int TenantIdValue { get; private set; }
    public string Name { get; private set; }
    public WarehouseType Type { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    
    // Domain ValueObjects (not mapped to database)
    [NotMapped]
    public TenantId TenantId => new TenantId(TenantIdValue);
    
    [NotMapped]
    public StreetAddress Address => new StreetAddress(Street, City, State, PostalCode, Country);
    
    [NotMapped]
    public Coordinates Location => new Coordinates(Latitude, Longitude);
    
    public string FullAddress => Address.FullAddress;

    // Constructor para EF Core
    private Warehouse()
    {
        TenantIdValue = 0;
        Name = string.Empty;
        Type = WarehouseType.Client;
        Street = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        Country = string.Empty;
        Latitude = 0;
        Longitude = 0;
        Phone = string.Empty;
        Email = string.Empty;
    }
    
    public Warehouse(TenantId tenantId, string name, WarehouseType type, 
                    string street, string city, string state, string postalCode, string country,
                    double latitude, double longitude, string phone, string email)
    {
        TenantIdValue = tenantId.Value;
        Name = name;
        Type = type;
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        Phone = phone;
        Email = email;
    }

    public Warehouse(CreateWarehouseCommand command)
    {
        TenantIdValue = command.TenantId.Value;
        Name = command.Name;
        Type = command.Type;
        Street = command.Street;
        City = command.City;
        State = command.State;
        PostalCode = command.PostalCode;
        Country = command.Country;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Phone = command.Phone;
        Email = command.Email;
    }

    public void UpdateWarehouse(string name, WarehouseType type,
                               string street, string city, string state, string postalCode, string country,
                               double latitude, double longitude, string phone, string email)
    {
        Name = name;
        Type = type;
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        Phone = phone;
        Email = email;
    }
}
