using  Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using  Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using  Alumware.Tracklab.API.Resource.Domain.Model.Entities;
using System.Text.RegularExpressions;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public partial class Vehicle
{
    public long Id { get; private set; }
    public long TenantId { get; private set; }
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Tenant { get; set; } = null!;
    public string LicensePlate { get; private set; } = null!;
    public decimal LoadCapacity { get; private set; }
    public int PaxCapacity { get; private set; }
    public EVehicleStatus Status { get; private set; }
    public Coordinates Location { get; private set; } = null!;
    public List<VehicleImage> Images { get; private set; } = new();
    public decimal Tonnage { get; private set; }
    
    public Vehicle() { }

    public Vehicle(CreateVehicleCommand command)
    {
        // Validación de placa: ABC-123
        if (!Regex.IsMatch(command.LicensePlate, @"^[A-Z]{3}-\d{3}$"))
            throw new ArgumentException("License plate must be in format ABC-123 (3 uppercase letters, hyphen, 3 digits).", nameof(command.LicensePlate));
        // Validación de capacidad de carga
        if (command.LoadCapacity < 10 || command.LoadCapacity > 50)
            throw new ArgumentException("Load capacity must be between 10 and 50.", nameof(command.LoadCapacity));
        if (command.Tonnage <= 0)
            throw new ArgumentException("Tonnage must be greater than 0.", nameof(command.Tonnage));
        LicensePlate = command.LicensePlate;
        LoadCapacity = command.LoadCapacity;
        PaxCapacity = command.PaxCapacity;
        Location = command.Location;
        Status = EVehicleStatus.Available;
        Images = new List<VehicleImage>();
        Tonnage = command.Tonnage;
    }

    public void UpdateStatus(UpdateVehicleStatusCommand command)
    {
        if (command.NewStatus != EVehicleStatus.Available && command.NewStatus != EVehicleStatus.NotAvailable)
            throw new ArgumentException("Status must be either 'Available' or 'NotAvailable'", nameof(command.NewStatus));
        Status = command.NewStatus;
    }

    public void UpdateInfo(UpdateVehicleInfoCommand command)
    {
        LicensePlate = command.NewLicensePlate;
        LoadCapacity = command.NewLoadCapacity;
        PaxCapacity = command.NewPaxCapacity;
        Location = command.NewLocation;
    }

    public void Delete(DeleteVehicleCommand command)
    {
    }

    public void AddImage(string imageUrl, string publicId)
    {
        if (!CanAddImage())
            throw new InvalidOperationException("A vehicle cannot have more than 3 images.");
        
        var vehicleImage = new VehicleImage(Id, imageUrl, publicId);
        Images.Add(vehicleImage);
    }
    
    public void RemoveImage(string publicId)
    {
        var image = Images.FirstOrDefault(img => img.PublicId == publicId);
        if (image != null)
        {
            Images.Remove(image);
        }
    }
    
    public void UpdateImage(string oldPublicId, string newImageUrl, string newPublicId)
    {
        var image = Images.FirstOrDefault(img => img.PublicId == oldPublicId);
        if (image != null)
        {
            image.UpdateImageUrl(newImageUrl, newPublicId);
        }
    }
    
    public bool CanAddImage()
    {
        return Images.Count < 3;
    }

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
    }
}
