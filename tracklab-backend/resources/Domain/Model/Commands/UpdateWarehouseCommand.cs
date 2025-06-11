using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;

namespace TrackLab.Resources.Domain.Model.Commands;

/// <summary>
/// Command to update an existing warehouse
/// </summary>
/// <param name="Id">Warehouse identifier</param>
/// <param name="TenantId">Tenant identifier</param>
/// <param name="Name">Warehouse name</param>
/// <param name="Type">Warehouse type</param>
/// <param name="Street">Street address</param>
/// <param name="City">City name</param>
/// <param name="State">State or province</param>
/// <param name="PostalCode">Postal code</param>
/// <param name="Country">Country name</param>
/// <param name="Latitude">Latitude coordinate</param>
/// <param name="Longitude">Longitude coordinate</param>
/// <param name="Phone">Phone number</param>
/// <param name="Email">Email address</param>
public record UpdateWarehouseCommand(
    int Id,
    TenantId TenantId,
    string Name,
    WarehouseType Type,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    double Latitude,
    double Longitude,
    string Phone,
    string Email); 