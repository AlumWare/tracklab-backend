using TrackLab.Resources.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace TrackLab.Resources.Interfaces.REST.Resources;

/// <summary>
/// Create warehouse resource for API requests
/// </summary>
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
public record CreateWarehouseResource(
    [Required] string Name,
    [Required] WarehouseType Type,
    [Required] string Street,
    [Required] string City,
    [Required] string State,
    [Required] string PostalCode,
    [Required] string Country,
    [Required] double Latitude,
    [Required] double Longitude,
    string Phone,
    string Email); 