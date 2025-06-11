namespace TrackLab.Resources.Domain.Model.ValueObjects;

/// <summary>
/// Street address value object
/// </summary>
/// <param name="Street">Street name and number</param>
/// <param name="City">City name</param>
/// <param name="State">State or province</param>
/// <param name="PostalCode">Postal or ZIP code</param>
/// <param name="Country">Country name</param>
public readonly record struct StreetAddress(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country)
{
    public string FullAddress => $"{Street}, {City}, {State} {PostalCode}, {Country}";
    
    public override string ToString() => FullAddress;
} 