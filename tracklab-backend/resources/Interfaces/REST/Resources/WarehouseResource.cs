using TrackLab.Resources.Domain.Model.ValueObjects;

namespace TrackLab.Resources.Interfaces.REST.Resources;

/// <summary>
/// Warehouse resource for API responses
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
/// <param name="FullAddress">Complete address</param>
/// <param name="Latitude">Latitude coordinate</param>
/// <param name="Longitude">Longitude coordinate</param>
/// <param name="Phone">Phone number</param>
/// <param name="Email">Email address</param>
public record WarehouseResource(
    int Id,
    int TenantId,
    string Name,
    string Type,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    string FullAddress,
    double Latitude,
    double Longitude,
    string Phone,
    string Email); 