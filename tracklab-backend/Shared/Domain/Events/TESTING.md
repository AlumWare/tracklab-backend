# Testing del Sistema de Eventos de Dominio

Este documento muestra cómo testear el sistema de eventos de dominio implementado en TrackLab.

## Ejemplos de Pruebas Unitarias

### 1. Testear el Event Handler

```csharp
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Alumware.Tracklab.API.Order.Domain.Events;
using Alumware.Tracklab.API.Order.Application.Internal.EventHandlers;

public class OrderCreatedEventHandlerTests
{
    private readonly Mock<ILogger<OrderCreatedEventHandler>> _loggerMock;
    private readonly OrderCreatedEventHandler _handler;

    public OrderCreatedEventHandlerTests()
    {
        _loggerMock = new Mock<ILogger<OrderCreatedEventHandler>>();
        _handler = new OrderCreatedEventHandler(_loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldLogOrderCreation_WhenEventIsPublished()
    {
        // Arrange
        var orderCreatedEvent = new OrderCreatedEvent(
            orderId: 1,
            customerId: 123,
            totalAmount: 100.50m,
            currency: "USD"
        );

        // Act
        await _handler.Handle(orderCreatedEvent, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Order 1")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeast(1)
        );
    }

    [Fact]
    public async Task Handle_ShouldCompleteSuccessfully_WhenValidEventIsProvided()
    {
        // Arrange
        var orderCreatedEvent = new OrderCreatedEvent(1, 123, 100.50m, "USD");

        // Act & Assert
        await _handler.Handle(orderCreatedEvent, CancellationToken.None);
        
        // No exception should be thrown
        Assert.True(true);
    }
}
```

### 2. Testear el Command Service con Eventos

```csharp
using Moq;
using Xunit;
using TrackLab.Shared.Domain.Events;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Events;

public class OrderCommandServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITenantContext> _tenantContextMock;
    private readonly Mock<IDomainEventDispatcher> _eventDispatcherMock;
    private readonly OrderCommandService _commandService;

    public OrderCommandServiceTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tenantContextMock = new Mock<ITenantContext>();
        _eventDispatcherMock = new Mock<IDomainEventDispatcher>();
        
        _commandService = new OrderCommandService(
            _orderRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _tenantContextMock.Object,
            _eventDispatcherMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldPublishOrderCreatedEvent_WhenOrderIsCreated()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerId = 123,
            DeliveryAddress = "Test Address",
            // ... otros campos
        };

        _tenantContextMock.Setup(x => x.HasTenant).Returns(true);
        _tenantContextMock.Setup(x => x.CurrentTenantId).Returns(1);

        // Act
        var result = await _commandService.CreateAsync(command);

        // Assert
        _eventDispatcherMock.Verify(
            x => x.PublishAsync(It.Is<OrderCreatedEvent>(e => 
                e.OrderId == result.Id && 
                e.CustomerId == command.CustomerId)),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistOrder_BeforePublishingEvent()
    {
        // Arrange
        var command = new CreateOrderCommand
        {
            CustomerId = 123,
            DeliveryAddress = "Test Address"
        };

        var callOrder = new List<string>();
        
        _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Order>()))
            .Callback(() => callOrder.Add("AddAsync"));
            
        _unitOfWorkMock.Setup(x => x.CompleteAsync())
            .Callback(() => callOrder.Add("CompleteAsync"));
            
        _eventDispatcherMock.Setup(x => x.PublishAsync(It.IsAny<OrderCreatedEvent>()))
            .Callback(() => callOrder.Add("PublishAsync"));

        // Act
        await _commandService.CreateAsync(command);

        // Assert
        Assert.Equal(new[] { "AddAsync", "CompleteAsync", "PublishAsync" }, callOrder);
    }
}
```

### 3. Testear el Domain Event Dispatcher

```csharp
using Microsoft.Extensions.Logging;
using MediatR;
using Moq;
using Xunit;
using TrackLab.Shared.Infrastructure.Events;
using TrackLab.Shared.Domain.Events;

public class DomainEventDispatcherTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<DomainEventDispatcher>> _loggerMock;
    private readonly DomainEventDispatcher _dispatcher;

    public DomainEventDispatcherTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<DomainEventDispatcher>>();
        _dispatcher = new DomainEventDispatcher(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task PublishAsync_ShouldCallMediatorPublish_WhenEventIsProvided()
    {
        // Arrange
        var domainEvent = new TestDomainEvent();

        // Act
        await _dispatcher.PublishAsync(domainEvent);

        // Assert
        _mediatorMock.Verify(x => x.Publish(domainEvent, default), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_ShouldLogInformation_WhenEventIsPublished()
    {
        // Arrange
        var domainEvent = new TestDomainEvent();

        // Act
        await _dispatcher.PublishAsync(domainEvent);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Publishing domain event")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeast(1)
        );
    }

    [Fact]
    public async Task PublishAsync_ShouldLogError_WhenMediatorThrowsException()
    {
        // Arrange
        var domainEvent = new TestDomainEvent();
        var exception = new Exception("Test exception");
        
        _mediatorMock.Setup(x => x.Publish(It.IsAny<INotification>(), default))
            .ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _dispatcher.PublishAsync(domainEvent));
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error publishing domain event")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishAllEvents_WhenMultipleEventsAreProvided()
    {
        // Arrange
        var events = new List<IDomainEvent>
        {
            new TestDomainEvent(),
            new TestDomainEvent(),
            new TestDomainEvent()
        };

        // Act
        await _dispatcher.PublishAsync(events);

        // Assert
        _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), default), Times.Exactly(3));
    }

    // Clase de prueba para los tests
    private class TestDomainEvent : DomainEventBase
    {
        public TestDomainEvent() : base(1) { }
    }
}
```

### 4. Test de Integración

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using TrackLab.Shared.Infrastructure.Events.Configuration;
using TrackLab.Shared.Domain.Events;

public class DomainEventsIntegrationTests : IClassFixture<TestApplicationFactory>
{
    private readonly TestApplicationFactory _factory;

    public DomainEventsIntegrationTests(TestApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DomainEventSystem_ShouldBeConfiguredCorrectly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dispatcher = scope.ServiceProvider.GetService<IDomainEventDispatcher>();

        // Assert
        Assert.NotNull(dispatcher);
        Assert.IsType<DomainEventDispatcher>(dispatcher);
    }

    [Fact]
    public async Task DomainEventSystem_ShouldPublishAndHandleEvents()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();
        var testEvent = new TestDomainEvent();

        // Act
        await dispatcher.PublishAsync(testEvent);

        // Assert
        // Verificar que el evento fue manejado correctamente
        // Esto dependería de cómo configures tu handler de prueba
    }
}

public class TestApplicationFactory : IDisposable
{
    public IServiceProvider Services { get; private set; }

    public TestApplicationFactory()
    {
        var services = new ServiceCollection();
        
        // Configurar los servicios necesarios para las pruebas
        services.AddLogging();
        services.AddDomainEvents(typeof(TestApplicationFactory).Assembly);
        
        Services = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (Services is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
```

## Configuración de Pruebas

### 1. Paquetes NuGet Necesarios

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="xunit" Version="2.6.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
<PackageReference Include="Moq" Version="4.20.69" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />
```

### 2. Configuración de TestHost

```csharp
services.AddDomainEvents(typeof(TestStartup).Assembly);
```

## Mejores Prácticas para Testing

1. **Mocking**: Usa mocks para dependencies externas
2. **Verificación de Orden**: Verifica que los eventos se publican después de persistir
3. **Aislamiento**: Cada test debe ser independiente
4. **Datos de Prueba**: Usa builders o factories para crear objetos de prueba
5. **Assertions Específicas**: Verifica propiedades específicas de los eventos
6. **Coverage**: Asegúrate de cubrir casos de error y edge cases

## Estructura de Archivos de Testing

```
Tests/
├── Unit/
│   ├── Events/
│   │   ├── OrderCreatedEventHandlerTests.cs
│   │   ├── DomainEventDispatcherTests.cs
│   │   └── ...
│   └── Services/
│       ├── OrderCommandServiceTests.cs
│       └── ...
├── Integration/
│   ├── DomainEventsIntegrationTests.cs
│   └── ...
└── TestUtilities/
    ├── TestApplicationFactory.cs
    ├── EventTestHelpers.cs
    └── ...
```

Este enfoque te permitirá tener una cobertura completa del sistema de eventos y garantizar que funcione correctamente en todos los escenarios. 