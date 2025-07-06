# Sistema de Eventos de Dominio

Este sistema permite implementar eventos de dominio en la aplicación TrackLab usando el patrón MediatR.

## Componentes Principales

### 1. Interfaces Base
- `IDomainEvent`: Interfaz base para todos los eventos de dominio
- `DomainEventBase`: Clase base abstracta que implementa propiedades comunes
- `IDomainEventDispatcher`: Interfaz para publicar eventos
- `IEventHandler<T>`: Interfaz para crear handlers de eventos

### 2. Implementaciones
- `DomainEventDispatcher`: Implementación que usa MediatR para publicar eventos

## Cómo Crear un Evento de Dominio

### Paso 1: Definir el Evento
```csharp
using TrackLab.Shared.Domain.Events;

public class OrderCreatedEvent : DomainEventBase
{
    public long OrderId { get; }
    public long CustomerId { get; }
    public decimal TotalAmount { get; }
    public string Currency { get; }

    public OrderCreatedEvent(long orderId, long customerId, decimal totalAmount, string currency)
        : base(version: 1)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Currency = currency;
    }
}
```

### Paso 2: Crear el Handler
```csharp
using TrackLab.Shared.Domain.Events;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    
    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order {OrderId} created with amount {Amount}", 
            notification.OrderId, notification.TotalAmount);
        
        // Lógica del handler aquí
        // - Enviar notificaciones
        // - Actualizar vistas
        // - Iniciar procesos
        // - etc.
    }
}
```

### Paso 3: Publicar el Evento
```csharp
public class OrderCommandService
{
    private readonly IDomainEventDispatcher _eventDispatcher;
    
    public OrderCommandService(IDomainEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }
    
    public async Task<Order> CreateAsync(CreateOrderCommand command)
    {
        var order = new Order(command);
        
        // Persistir la orden
        await _orderRepository.AddAsync(order);
        await _unitOfWork.CompleteAsync();
        
        // Publicar el evento
        var orderCreatedEvent = new OrderCreatedEvent(
            order.Id, 
            order.CustomerId, 
            order.GetTotalAmount(), 
            order.Currency
        );
        
        await _eventDispatcher.PublishAsync(orderCreatedEvent);
        
        return order;
    }
}
```

## Configuración

El sistema se configura automáticamente en `Program.cs`:

```csharp
// Add Domain Events System
builder.Services.AddDomainEvents(
    typeof(Program).Assembly, // Current assembly
    typeof(IDomainEventDispatcher).Assembly // Shared assembly
);
```

## Ventajas del Sistema

1. **Desacoplamiento**: Los módulos no dependen directamente entre sí
2. **Extensibilidad**: Fácil agregar nuevos handlers sin modificar código existente
3. **Testabilidad**: Cada handler puede ser testado independientemente
4. **Auditabilidad**: Registro automático de todos los eventos
5. **Flexibilidad**: Múltiples handlers pueden responder al mismo evento

## Casos de Uso Comunes

### 1. Notificaciones
```csharp
public class OrderCreatedNotificationHandler : IEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Enviar email de confirmación
        // Crear notificación push
        // Actualizar dashboard
    }
}
```

### 2. Auditoría
```csharp
public class OrderCreatedAuditHandler : IEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Crear registro de auditoría
        // Log para compliance
        // Métricas de negocio
    }
}
```

### 3. Integración con Sistemas Externos
```csharp
public class OrderCreatedExternalHandler : IEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Notificar ERP
        // Actualizar CRM
        // Sincronizar con warehouse
    }
}
```

## Mejores Prácticas

1. **Nombres Descriptivos**: Los eventos deben usar nombres en pasado (OrderCreated, UserRegistered)
2. **Inmutabilidad**: Los eventos deben ser inmutables
3. **Información Completa**: Incluir toda la información necesaria para los handlers
4. **Versionado**: Usar el campo `Version` para evolución de eventos
5. **Logging**: Los handlers deben logear sus acciones
6. **Idempotencia**: Los handlers deben ser idempotentes cuando sea posible
7. **Manejo de Errores**: Implementar retry logic si es necesario

## Estructura de Archivos Recomendada

```
ModuleName/
├── Domain/
│   ├── Events/
│   │   ├── OrderCreatedEvent.cs
│   │   ├── OrderStatusChangedEvent.cs
│   │   └── ...
└── Application/
    ├── Internal/
    │   ├── EventHandlers/
    │   │   ├── OrderCreatedEventHandler.cs
    │   │   ├── OrderStatusChangedEventHandler.cs
    │   │   └── ...
```

Este sistema te permite implementar una arquitectura orientada a eventos de forma simple y escalable. 