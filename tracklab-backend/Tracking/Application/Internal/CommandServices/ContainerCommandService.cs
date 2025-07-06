using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class ContainerCommandService : IContainerCommandService
{
    private readonly IContainerRepository _containerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ContainerCommandService(
        IContainerRepository containerRepository,
        IOrderRepository orderRepository,
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _containerRepository = containerRepository;
        _orderRepository = orderRepository;
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Container> CreateAsync(CreateContainerCommand command)
    {
        // Validar que la orden existe y obtener sus productos
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new ArgumentException($"La orden con ID {command.OrderId} no existe.");
        
        // Validar que la orden tenga productos
        if (order.OrderItems == null || !order.OrderItems.Any())
            throw new ArgumentException($"La orden con ID {command.OrderId} no tiene productos.");
        
        // Validar que el almacén existe
        var warehouse = await _warehouseRepository.FindByIdAsync(command.WarehouseId);
        if (warehouse == null)
            throw new ArgumentException($"El almacén con ID {command.WarehouseId} no existe.");
        
        var container = new Container(command);
        
        // Establecer los productos de la orden en el contenedor
        container.SetShipItemsFromOrder(order.OrderItems);
        
        await _containerRepository.AddAsync(container);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return container;
    }

    public async Task<Container> UpdateCurrentNodeAsync(UpdateContainerNodeCommand command)
    {
        var container = await _containerRepository.GetByIdAsync(new GetContainerByIdQuery(command.ContainerId));
        if (container == null)
            throw new KeyNotFoundException($"Container {command.ContainerId} no encontrado.");
        container.UpdateCurrentNode(command);
        _containerRepository.Update(container);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return container;
    }
} 