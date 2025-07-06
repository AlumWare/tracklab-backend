# QR Code Service

Este servicio gestiona la generación de códigos QR para contenedores en el contexto de Tracking utilizando QRCoder y Cloudinary.

## Configuración

### Registro en Program.cs
```csharp
builder.Services.AddTrackingServices();
```

## Uso

### Inyección de dependencias
```csharp
public class ContainerCommandService
{
    private readonly IQrCodeService _qrCodeService;
    
    public ContainerCommandService(IQrCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }
}
```

### Generar QR para un contenedor
```csharp
var qrCode = await _qrCodeService.GenerateQrCodeAsync(containerId, "https://tracklab.com/track");
container.AssignQrCode(qrCode);
```

## Endpoints de Prueba

### Generar QR para un contenedor específico
```
POST /api/QrCode/{containerId}
```

Ejemplo:
```bash
curl -X POST "https://localhost:7000/api/QrCode/123" -H "accept: application/json"
```

### Generar QR personalizado
```
POST /api/QrCode/custom
Content-Type: application/json

{
  "data": "12345",
  "size": 400
}
```

## Características

- **Value Object**: QR es un Value Object inmutable
- **QRCoder**: Genera QR codes localmente sin API externa
- **Cloudinary**: Almacena imágenes QR en la nube
- **Tamaño optimizado**: 400x400 px para impresión 3x3cm a 300 DPI
- **Solo ID**: El QR contiene únicamente el ID del contenedor
- **Carpeta organizada**: Se almacena en `qr-codes/` en Cloudinary

## Base de datos

El QR se almacena como un Value Object en la tabla `containers`:
- `qr_code_url`: URL del QR en Cloudinary
- `qr_code_generated_at`: Timestamp de generación 