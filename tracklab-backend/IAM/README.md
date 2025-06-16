# TrackLab IAM Context

## 🏗️ **Arquitectura**

El contexto IAM (Identity and Access Management) implementa autenticación y autorización multitenant usando:

- **Domain-Driven Design (DDD)** - Separación clara de capas
- **Multitenancy** - Aislamiento de datos por tenant
- **JWT Authentication** - Tokens seguros con claims de tenant
- **Role-Based Access Control** - 5 roles predefinidos

## 🏢 **Tipos de Tenant**

### **1. Tenant Logístico** 
- Empresas principales del sistema
- Usuarios con roles: `ADMIN`, `OPERATOR`, `SUPERVISOR`
- Funcionalidades completas

### **2. Tenant Cliente**
- Empresas cliente
- Solo rol: `CLIENT`
- Funcionalidades limitadas (órdenes de compra)

### **3. Tenant Proveedor**
- Empresas proveedor  
- Solo rol: `PROVIDER`
- Funcionalidades limitadas (publicar productos)

## 🔐 **Roles Disponibles**

```csharp
Role.Admin      // Administrador del tenant
Role.Operator   // Operador logístico
Role.Supervisor // Supervisor de operaciones
Role.Client     // Cliente (solo órdenes)
Role.Provider   // Proveedor (solo productos)
```

## 🚀 **Endpoints**

### **1. Registro de Empresa (Sign Up)**
```http
POST /api/v1/authentication/sign-up
Content-Type: application/json

{
  "ruc": "10744232619",
  "legalName": "TrackLab Solutions SAC",
  "commercialName": "TrackLab",
  "address": "Av. Javier Prado 123",
  "city": "Lima",
  "country": "Peru",
  "tenantPhone": "980370785",
  "tenantEmail": "info@tracklab.com",
  "website": "https://tracklab.com",
  "username": "admin",
  "password": "123456",
  "email": "admin@tracklab.com",
  "firstName": "Juan",
  "lastName": "Pérez",
  "tenantType": "LOGISTIC"  // LOGISTIC, CLIENT, PROVIDER
}
```

**Respuesta:**
```json
{
  "id": 1,
  "username": "admin",
  "email": "admin@tracklab.com",
  "firstName": "Juan",
  "lastName": "Pérez",
  "fullName": "Juan Pérez",
  "active": true,
  "tenantId": 1,
  "roles": ["ADMIN"]
}
```

### **2. Iniciar Sesión (Sign In)**
```http
POST /api/v1/authentication/sign-in
Content-Type: application/json

{
  "username": "admin",
  "password": "123456"
}
```

**Respuesta:**
```json
{
  "id": 1,
  "username": "admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### **3. Crear Usuario (Solo Admin)**
```http
POST /api/v1/authentication/users
Authorization: Bearer {token}
Content-Type: application/json

{
  "username": "operador1",
  "password": "123456",
  "email": "operador1@tracklab.com",
  "firstName": "María",
  "lastName": "García",
  "roles": ["OPERATOR"]
}
```

### **4. Obtener Perfil Actual**
```http
GET /api/v1/authentication/profile
Authorization: Bearer {token}
```

### **5. Listar Usuarios del Tenant**
```http
GET /api/v1/authentication/users
Authorization: Bearer {token}
```

### **6. Obtener Usuario por ID**
```http
GET /api/v1/authentication/users/1
Authorization: Bearer {token}
```

## 🔧 **Configuración**

### **1. appsettings.json**
```json
{
  "TokenSettings": {
    "Secret": "your-super-secret-key-here-must-be-at-least-32-characters",
    "Issuer": "TrackLab",
    "Audience": "TrackLab-Users",
    "ExpirationInMinutes": 1440
  }
}
```

### **2. Program.cs**
```csharp
using TrackLab.IAM.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add IAM services
builder.Services.AddIamConfiguration(builder.Configuration);

var app = builder.Build();

// Configure IAM middleware
app.UseIamConfiguration();

app.Run();
```

## 🏗️ **Flujo de Autenticación**

1. **Registro** → Crea Tenant + Usuario Admin
2. **Login** → Valida credenciales + Genera JWT con `tenant_id`
3. **Request** → Middleware extrae `tenant_id` del JWT
4. **Context** → Se establece `TenantContext` automáticamente
5. **Repository** → Filtra datos por tenant automáticamente

## 🔒 **Seguridad Multitenant**

- ✅ **JWT con tenant_id** - Cada token incluye el tenant
- ✅ **Middleware automático** - Extrae y valida tenant
- ✅ **Repository filtering** - Filtra automáticamente por tenant
- ✅ **Cross-tenant protection** - Imposible acceder a datos de otros tenants
- ✅ **Role-based access** - Control granular por roles

## 📝 **Ejemplos de Uso**

### **Empresa Logística**
```json
{
  "tenantType": "LOGISTIC",
  "roles": ["ADMIN"]  // Puede crear usuarios con cualquier rol
}
```

### **Empresa Cliente**
```json
{
  "tenantType": "CLIENT", 
  "roles": ["CLIENT"]  // Solo funciones de cliente
}
```

### **Empresa Proveedor**
```json
{
  "tenantType": "PROVIDER",
  "roles": ["PROVIDER"]  // Solo funciones de proveedor
}
```

## 🚨 **Manejo de Errores**

Todos los endpoints devuelven errores en formato:
```json
{
  "message": "Username is already taken"
}
```

**Códigos de Estado:**
- `200` - OK
- `201` - Created
- `400` - Bad Request (validación)
- `401` - Unauthorized (token inválido)
- `404` - Not Found
- `500` - Internal Server Error 