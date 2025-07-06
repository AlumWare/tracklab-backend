# Servicio de Imágenes de Cloudinary

Este servicio permite subir, obtener y eliminar imágenes utilizando Cloudinary como proveedor de almacenamiento en la nube.

## Configuración

### 1. Configurar Cloudinary en appsettings.json

```json
{
  "Cloudinary": {
    "CloudName": "tu-cloud-name",
    "ApiKey": "tu-api-key",
    "ApiSecret": "tu-api-secret",
    "MaxImageSizeInBytes": 5242880,
    "AllowedFormats": ["jpg", "jpeg", "png", "gif", "webp"],
    "DefaultFolder": "tracklab"
  }
}
```

### 2. Registro del servicio

El servicio se registra automáticamente en `Program.cs` con:

```csharp
builder.Services.AddCloudinaryImageService(builder.Configuration);
```

## Uso del Servicio

### Inyección de Dependencias

```csharp
[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    
    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }
}
```

### Subir Imagen desde FormFile

```csharp
[HttpPost("upload")]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    try
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No se ha proporcionado ningún archivo");
        }

        using var stream = file.OpenReadStream();
        var imageUrl = await _imageService.UploadImageAsync(stream, file.FileName, "productos");
        
        return Ok(new { ImageUrl = imageUrl });
    }
    catch (ImageUploadException ex)
    {
        return BadRequest(ex.Message);
    }
}
```

### Subir Imagen desde Bytes

```csharp
[HttpPost("upload-bytes")]
public async Task<IActionResult> UploadImageFromBytes(byte[] imageBytes, string fileName)
{
    try
    {
        var imageUrl = await _imageService.UploadImageAsync(imageBytes, fileName, "empleados");
        
        return Ok(new { ImageUrl = imageUrl });
    }
    catch (ImageUploadException ex)
    {
        return BadRequest(ex.Message);
    }
}
```

### Eliminar Imagen

```csharp
[HttpDelete("{publicId}")]
public async Task<IActionResult> DeleteImage(string publicId)
{
    try
    {
        var success = await _imageService.DeleteImageAsync(publicId);
        
        if (success)
        {
            return Ok(new { Message = "Imagen eliminada correctamente" });
        }
        else
        {
            return NotFound("Imagen no encontrada");
        }
    }
    catch (ImageDeleteException ex)
    {
        return BadRequest(ex.Message);
    }
}
```

### Extraer Public ID de URL

```csharp
[HttpGet("public-id")]
public IActionResult GetPublicId([FromQuery] string imageUrl)
{
    try
    {
        var publicId = _imageService.ExtractPublicIdFromUrl(imageUrl);
        
        return Ok(new { PublicId = publicId });
    }
    catch (InvalidImageUrlException ex)
    {
        return BadRequest(ex.Message);
    }
}
```

## Ejemplo de Uso en un Servicio de Dominio

```csharp
public class ProductCommandService : IProductCommandService
{
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    
    public ProductCommandService(
        IProductRepository productRepository,
        IImageService imageService)
    {
        _productRepository = productRepository;
        _imageService = imageService;
    }
    
    public async Task<Product> CreateProductAsync(CreateProductCommand command)
    {
        string imageUrl = null;
        
        // Subir imagen si se proporciona
        if (command.ImageFile != null)
        {
            using var stream = command.ImageFile.OpenReadStream();
            imageUrl = await _imageService.UploadImageAsync(
                stream, 
                command.ImageFile.FileName, 
                "productos"
            );
        }
        
        var product = new Product(
            command.Name,
            command.Description,
            command.Price,
            imageUrl
        );
        
        await _productRepository.AddAsync(product);
        await _productRepository.SaveAsync();
        
        return product;
    }
}
```

## Excepciones

El servicio lanza las siguientes excepciones específicas:

- `ImageUploadException`: Error al subir la imagen
- `ImageDeleteException`: Error al eliminar la imagen
- `InvalidImageUrlException`: URL de imagen inválida
- `ImageServiceException`: Error general del servicio

## Características

- **Validación de tamaño**: Valida que las imágenes no excedan el tamaño máximo configurado
- **Formatos permitidos**: Valida los formatos de imagen permitidos
- **Transformaciones automáticas**: Aplica optimizaciones automáticas (quality: auto, format: auto)
- **Logging**: Registra todas las operaciones para auditoría
- **Gestión de errores**: Manejo robusto de errores con excepciones específicas

## Configuración Avanzada

### Transformaciones Personalizadas

El servicio aplica automáticamente las siguientes transformaciones:

```csharp
new Transformation()
    .Quality("auto")
    .FetchFormat("auto")
```

### Estructura de Archivos

Las imágenes se organizan en la siguiente estructura:

```
tracklab/
├── productos/
│   ├── producto_20240101123456.jpg
│   └── producto_20240101123457.png
├── empleados/
│   ├── empleado_20240101123458.jpg
│   └── empleado_20240101123459.png
└── vehiculos/
    ├── vehiculo_20240101123460.jpg
    └── vehiculo_20240101123461.png
```

El nombre del archivo incluye un timestamp para evitar conflictos. 