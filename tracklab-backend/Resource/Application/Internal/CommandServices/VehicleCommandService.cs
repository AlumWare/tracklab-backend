using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class VehicleCommandService : IVehicleCommandService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly IImageService _imageService;

    public VehicleCommandService(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        IImageService imageService)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _imageService = imageService;
    }

    public async Task<Vehicle> Handle(CreateVehicleCommand command)
    {
        var vehicle = new Vehicle(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            vehicle.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }
        
        await _vehicleRepository.AddAsync(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios primero para obtener el ID
        
        // Procesar imágenes si las hay
        if (command.Images != null && command.Images.Length > 0)
        {
            if (command.Images.Length > 3)
                throw new InvalidOperationException("Un vehículo no puede tener más de 3 imágenes.");
            
            foreach (var imageFile in command.Images)
            {
                using var stream = imageFile.OpenReadStream();
                var imageUrl = await _imageService.UploadImageAsync(stream, imageFile.FileName, "vehicles");
                var publicId = _imageService.ExtractPublicIdFromUrl(imageUrl);
                
                vehicle.AddImage(imageUrl, publicId);
            }
            
            _vehicleRepository.Update(vehicle);
            await _unitOfWork.CompleteAsync();
        }
        
        return vehicle;
    }

    public async Task Handle(UpdateVehicleStatusCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateStatus(command);
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(UpdateVehicleInfoCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateInfo(command);
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(DeleteVehicleCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        // Eliminar imágenes de Cloudinary antes de eliminar el vehículo
        foreach (var image in vehicle.Images)
        {
            await _imageService.DeleteImageAsync(image.PublicId);
        }

        _vehicleRepository.Remove(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(AddVehicleImageCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        if (!vehicle.CanAddImage())
            throw new InvalidOperationException("El vehículo ya tiene el máximo de 3 imágenes.");

        using var stream = command.ImageFile.OpenReadStream();
        var imageUrl = await _imageService.UploadImageAsync(stream, command.ImageFile.FileName, "vehicles");
        var publicId = _imageService.ExtractPublicIdFromUrl(imageUrl);

        vehicle.AddImage(imageUrl, publicId);
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(RemoveVehicleImageCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        var image = vehicle.Images.FirstOrDefault(img => img.PublicId == command.PublicId);
        if (image != null)
        {
            await _imageService.DeleteImageAsync(image.PublicId);
            vehicle.RemoveImage(command.PublicId);
            _vehicleRepository.Update(vehicle);
            await _unitOfWork.CompleteAsync();
        }
    }
}