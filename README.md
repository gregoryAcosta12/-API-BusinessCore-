
# 📊 BusinessCore API

## 🚀 API Backend Empresarial en Clean Architecture

---

## 📋 **Tabla de Contenidos**

- [Descripción General](#descripción-general)
- [Arquitectura](#arquitectura)
- [Patrones de Diseño Implementados](#patrones-de-diseño-implementados)
- [Buenas Prácticas](#buenas-prácticas)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Características Principales](#características-principales)
- [Endpoints de la API](#endpoints-de-la-api)
- [Instalación y Configuración](#instalación-y-configuración)
- [Uso de la API](#uso-de-la-api)
- [Documentación Swagger](#documentación-swagger)
- [Seguridad](#seguridad)
- [Rendimiento y Optimización](#rendimiento-y-optimización)
- [Pruebas](#pruebas)
- [Contribución](#contribución)
- [Licencia](#licencia)

---

## 📖 **Descripción General**

**BusinessCore API** es una API RESTful empresarial construida siguiendo los principios de **Clean Architecture** y **Domain-Driven Design (DDD)**. Proporciona un conjunto completo de endpoints para la gestión de productos, categorías, marcas, usuarios, órdenes, pagos, inventario y más.

### 🎯 **Objetivo del API**

Proveer una plataforma escalable, mantenible y segura para la gestión empresarial, permitiendo:

- **Gestión de Productos** con categorías, marcas y variantes
- **Gestión de Usuarios** con autenticación JWT y roles
- **Gestión de Órdenes** con estados y seguimiento
- **Gestión de Pagos** con múltiples métodos
- **Gestión de Inventario** con movimientos y alertas
- **Gestión de Clientes** con crédito y balances
- **Dashboard** con estadísticas y reportes en tiempo real
- **Auditoría** completa de todas las operaciones

---

## 🏗️ **Arquitectura**

### Clean Architecture

El proyecto está estructurado siguiendo los principios de **Clean Architecture** propuestos por Robert C. Martin (Uncle Bob), garantizando:
┌─────────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER │
│ (BusinessCore.API) │
│ ┌─────────────┐ ┌─────────────┐ ┌───────────────────┐ │
│ │ Controllers │ │ Middleware │ │ Extensions │ │
│ └─────────────┘ └─────────────┘ └───────────────────┘ │
└─────────────────────────────────────────────────────────────┘
│
▼
┌─────────────────────────────────────────────────────────────┐
│ APPLICATION LAYER │
│ (BusinessCore.Application) │
│ ┌─────────────┐ ┌─────────────┐ ┌───────────────────┐ │
│ │ Services │ │ DTOs │ │ Validators │ │
│ │ Interfaces │ │ Mappings │ │ Common │ │
│ └─────────────┘ └─────────────┘ └───────────────────┘ │
└─────────────────────────────────────────────────────────────┘
│
▼
┌─────────────────────────────────────────────────────────────┐
│ DOMAIN LAYER │
│ (BusinessCore.Domain) │
│ ┌─────────────┐ ┌─────────────┐ ┌───────────────────┐ │
│ │ Entities │ │ Enums │ │ Exceptions │ │
│ │ Interfaces │ │ Value │ │ Aggregates │ │
│ └─────────────┘ └─────────────┘ └───────────────────┘ │
└─────────────────────────────────────────────────────────────┘
│
▼
┌─────────────────────────────────────────────────────────────┐
│ INFRASTRUCTURE LAYER │
│ (BusinessCore.Infrastructure) │
│ ┌─────────────┐ ┌─────────────┐ ┌───────────────────┐ │
│ │ Repositories│ │ DbContext │ │ Configurations │ │
│ │ UnitOfWork│ │ Migrations│ │ External Services│ │
│ └─────────────┘ └─────────────┘ └───────────────────┘ │
└─────────────────────────────────────────────────────────────┘

text

### Dependencias

Las dependencias fluyen hacia adentro, siguiendo el **Principio de Inversión de Dependencias (DIP)**:
API → Application → Domain ← Infrastructure

text

- **API** depende de **Application** y **Infrastructure**
- **Application** depende de **Domain**
- **Infrastructure** depende de **Domain**
- **Domain** es independiente (no depende de nada)

---

## 🎨 **Patrones de Diseño Implementados**

| Patrón | Descripción | Ubicación |
|--------|-------------|-----------|
| **Repository Pattern** | Abstracción del acceso a datos | Domain/Interfaces + Infrastructure/Repositories |
| **Unit of Work** | Gestión de transacciones | Infrastructure/Repositories/UnitOfWork |
| **Dependency Injection** | Inversión de control | API/Extensions/ServiceExtensions |
| **Service Layer** | Lógica de negocio | Application/Services |
| **DTO Pattern** | Transferencia de datos | Application/DTOs |
| **AutoMapper** | Mapeo entre capas | Application/Mappings |
| **FluentValidation** | Validación de datos | Application/Validators |
| **Middleware Pipeline** | Procesamiento de peticiones | API/Middleware |
| **Global Exception Handler** | Manejo centralizado de errores | API/Middleware/GlobalExceptionMiddleware |
| **Factory Pattern** | Creación de objetos | Infrastructure/Repositories |
| **Strategy Pattern** | Estrategias de pago | Application/Services/PaymentService |
| **Observer Pattern** | Eventos y notificaciones | Application/Services/NotificationService |
| **Decorator Pattern** | Logging y caché | Application/Services |
| **Singleton** | Servicios compartidos | Application/Services |

---

## ✅ **Buenas Prácticas**

### 1. **Principios SOLID**

| Principio | Implementación |
|-----------|----------------|
| **S** - Single Responsibility | Cada clase tiene una única responsabilidad |
| **O** - Open/Closed | Extensiones sin modificar el código existente |
| **L** - Liskov Substitution | Uso de interfaces y herencia correcta |
| **I** - Interface Segregation | Interfaces específicas y pequeñas |
| **D** - Dependency Inversion | Dependencias en abstracciones, no en concretos |

### 2. **Arquitectura Limpia**

- ✅ Separación en 4 capas: API, Application, Domain, Infrastructure
- ✅ Domain independiente de cualquier framework
- ✅ Casos de uso en Application
- ✅ Persistencia en Infrastructure
- ✅ Presentación en API

### 3. **Código Limpio**

- ✅ Nombres descriptivos en clases, métodos y variables
- ✅ Métodos pequeños y enfocados
- ✅ Comentarios relevantes (en español)
- ✅ Uso de `async/await` para operaciones I/O
- ✅ Manejo adecuado de excepciones

### 4. **Seguridad**

- ✅ Autenticación JWT con refresh tokens
- ✅ Autorización basada en roles
- ✅ Validación de datos con FluentValidation
- ✅ Headers de seguridad (X-Content-Type-Options, X-Frame-Options, CSP)
- ✅ Encriptación de contraseñas con SHA256
- ✅ Correlation ID para rastreo de peticiones

### 5. **Rendimiento**

- ✅ Paginación en listados
- ✅ Compresión de respuestas (Gzip/Brotli)
- ✅ Asincronía en todas las operaciones I/O
- ✅ Uso de `AsNoTracking` en consultas de solo lectura
- ✅ Caché implementado (Redis/MemoryCache)

### 6. **Mantenibilidad**

- ✅ Código autodocumentado
- ✅ Separación de responsabilidades
- ✅ Fácil de extender y modificar
- ✅ Pruebas unitarias y de integración

---

## 🛠️ **Tecnologías Utilizadas**

### Backend

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET 8 | 8.0 | Framework principal |
| Entity Framework Core | 8.0 | ORM para acceso a datos |
| SQL Server | 2022 | Base de datos relacional |
| AutoMapper | 12.0 | Mapeo de objetos |
| FluentValidation | 11.9 | Validación de datos |
| JWT Bearer | 8.0 | Autenticación |
| Swagger/OpenAPI | 6.5 | Documentación de API |

### Paquetes NuGet

```xml
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
📁 Estructura del Proyecto
text
BusinessCore.sln
│
├── BusinessCore.Domain/                               [DOMAIN LAYER]
│   ├── Entities/
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Brand.cs
│   │   ├── ProductVariant.cs
│   │   ├── ProductImage.cs
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Payment.cs
│   │   ├── Invoice.cs
│   │   ├── InventoryMovement.cs
│   │   ├── Warehouse.cs
│   │   ├── Address.cs
│   │   └── Review.cs
│   │
│   ├── Enums/
│   │   ├── OrderStatus.cs
│   │   ├── PaymentStatus.cs
│   │   ├── InvoiceStatus.cs
│   │   ├── MovementType.cs
│   │   ├── UserRoleType.cs
│   │   ├── ProductStatus.cs
│   │   ├── AddressType.cs
│   │   ├── ReviewStatus.cs
│   │   ├── NotificationType.cs
│   │   ├── DiscountType.cs
│   │   ├── ShippingStatus.cs
│   │   ├── ReturnStatus.cs
│   │   ├── AuditActionType.cs
│   │   ├── FileType.cs
│   │   └── Gender.cs
│   │
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   ├── ValidationException.cs
│   │   ├── BusinessException.cs
│   │   ├── UnauthorizedException.cs
│   │   ├── ConflictException.cs
│   │   ├── ForbiddenException.cs
│   │   ├── BadRequestException.cs
│   │   ├── InfrastructureException.cs
│   │   ├── DatabaseException.cs
│   │   └── FileException.cs
│   │
│   └── Interfaces/
│       ├── IProductRepository.cs
│       ├── ICategoryRepository.cs
│       ├── IUserRepository.cs
│       ├── IOrderRepository.cs
│       ├── ICustomerRepository.cs
│       ├── IAddressRepository.cs
│       ├── IPaymentRepository.cs
│       ├── IReviewRepository.cs
│       ├── IInvoiceRepository.cs
│       ├── IInventoryRepository.cs
│       ├── IRoleRepository.cs
│       └── IUnitOfWork.cs
│
├── BusinessCore.Application/                           [APPLICATION LAYER]
│   ├── DTOs/
│   │   ├── Common/
│   │   │   ├── PagedResultDto.cs
│   │   │   ├── ApiResponseDto.cs
│   │   │   ├── ErrorResponseDto.cs
│   │   │   └── LookupDto.cs
│   │   │
│   │   ├── Products/
│   │   │   ├── ProductResponseDto.cs
│   │   │   ├── ProductCreateDto.cs
│   │   │   ├── ProductUpdateDto.cs
│   │   │   ├── ProductFilterDto.cs
│   │   │   ├── ProductImageDto.cs
│   │   │   ├── ProductImageCreateDto.cs
│   │   │   ├── ProductVariantDto.cs
│   │   │   └── ProductVariantCreateDto.cs
│   │   │
│   │   ├── Categories/
│   │   │   ├── CategoryResponseDto.cs
│   │   │   ├── CategoryCreateDto.cs
│   │   │   ├── CategoryUpdateDto.cs
│   │   │   └── CategoryFilterDto.cs
│   │   │
│   │   ├── Brands/
│   │   │   ├── BrandResponseDto.cs
│   │   │   ├── BrandCreateDto.cs
│   │   │   ├── BrandUpdateDto.cs
│   │   │   └── BrandFilterDto.cs
│   │   │
│   │   ├── Users/
│   │   │   ├── UserResponseDto.cs
│   │   │   ├── UserCreateDto.cs
│   │   │   ├── UserUpdateDto.cs
│   │   │   ├── LoginDto.cs
│   │   │   ├── RegisterDto.cs
│   │   │   ├── ChangePasswordDto.cs
│   │   │   ├── UserFilterDto.cs
│   │   │   └── LoginResponseDto.cs
│   │   │
│   │   ├── Orders/
│   │   │   ├── OrderResponseDto.cs
│   │   │   ├── OrderCreateDto.cs
│   │   │   ├── OrderUpdateDto.cs
│   │   │   ├── OrderItemDto.cs
│   │   │   ├── OrderItemCreateDto.cs
│   │   │   └── OrderFilterDto.cs
│   │   │
│   │   ├── Customers/
│   │   │   ├── CustomerResponseDto.cs
│   │   │   ├── CustomerCreateDto.cs
│   │   │   ├── CustomerUpdateDto.cs
│   │   │   └── CustomerFilterDto.cs
│   │   │
│   │   ├── Addresses/
│   │   │   ├── AddressResponseDto.cs
│   │   │   ├── AddressCreateDto.cs
│   │   │   └── AddressUpdateDto.cs
│   │   │
│   │   ├── Payments/
│   │   │   ├── PaymentResponseDto.cs
│   │   │   ├── PaymentCreateDto.cs
│   │   │   ├── PaymentUpdateDto.cs
│   │   │   └── PaymentFilterDto.cs
│   │   │
│   │   ├── Reviews/
│   │   │   ├── ReviewResponseDto.cs
│   │   │   ├── ReviewCreateDto.cs
│   │   │   ├── ReviewUpdateDto.cs
│   │   │   └── ReviewFilterDto.cs
│   │   │
│   │   ├── Invoices/
│   │   │   ├── InvoiceResponseDto.cs
│   │   │   ├── InvoiceCreateDto.cs
│   │   │   ├── InvoiceUpdateDto.cs
│   │   │   └── InvoiceFilterDto.cs
│   │   │
│   │   ├── Inventory/
│   │   │   ├── InventoryMovementResponseDto.cs
│   │   │   ├── InventoryMovementCreateDto.cs
│   │   │   └── InventoryFilterDto.cs
│   │   │
│   │   └── Dashboard/
│   │       ├── DashboardStatsDto.cs
│   │       ├── SalesOverviewDto.cs
│   │       ├── OrderStatsDto.cs
│   │       ├── SalesReportDto.cs
│   │       ├── MonthlySalesDto.cs
│   │       ├── TopProductDto.cs
│   │       ├── TopCategoryDto.cs
│   │       ├── TopCustomerDto.cs
│   │       └── CustomerStatsDto.cs
│   │
│   ├── Interfaces/
│   │   ├── IProductService.cs
│   │   ├── ICategoryService.cs
│   │   ├── IBrandService.cs
│   │   ├── IUserService.cs
│   │   ├── IOrderService.cs
│   │   ├── ICustomerService.cs
│   │   ├── IAddressService.cs
│   │   ├── IPaymentService.cs
│   │   ├── IReviewService.cs
│   │   ├── IInvoiceService.cs
│   │   ├── IInventoryService.cs
│   │   ├── IRoleService.cs
│   │   ├── IDashboardService.cs
│   │   ├── IEmailService.cs
│   │   ├── IFileService.cs
│   │   ├── IAuditService.cs
│   │   └── ICacheService.cs
│   │
│   ├── Mappings/
│   │   ├── ProductMappingProfile.cs
│   │   ├── CategoryMappingProfile.cs
│   │   ├── BrandMappingProfile.cs
│   │   ├── UserMappingProfile.cs
│   │   ├── OrderMappingProfile.cs
│   │   ├── CustomerMappingProfile.cs
│   │   ├── AddressMappingProfile.cs
│   │   ├── PaymentMappingProfile.cs
│   │   ├── ReviewMappingProfile.cs
│   │   ├── InvoiceMappingProfile.cs
│   │   ├── InventoryMappingProfile.cs
│   │   ├── RoleMappingProfile.cs
│   │   ├── CommonMappingProfile.cs
│   │   ├── AuditMappingProfile.cs
│   │   └── WarehouseMappingProfile.cs
│   │
│   ├── Services/
│   │   ├── ProductService.cs
│   │   ├── CategoryService.cs
│   │   ├── BrandService.cs
│   │   ├── UserService.cs
│   │   ├── OrderService.cs
│   │   ├── CustomerService.cs
│   │   ├── AddressService.cs
│   │   ├── PaymentService.cs
│   │   ├── ReviewService.cs
│   │   ├── InvoiceService.cs
│   │   ├── InventoryService.cs
│   │   └── RoleService.cs
│   │
│   └── Validators/
│       ├── ProductValidator.cs
│       ├── CategoryValidator.cs
│       ├── BrandValidator.cs
│       ├── UserValidator.cs
│       ├── OrderValidator.cs
│       ├── CustomerValidator.cs
│       ├── AddressValidator.cs
│       ├── PaymentValidator.cs
│       ├── ReviewValidator.cs
│       ├── InvoiceValidator.cs
│       ├── InventoryValidator.cs
│       └── RoleValidator.cs
│
├── BusinessCore.Infrastructure/                       [INFRASTRUCTURE LAYER]
│   ├── Configurations/
│   │   ├── ProductConfiguration.cs
│   │   ├── CategoryConfiguration.cs
│   │   ├── BrandConfiguration.cs
│   │   ├── UserConfiguration.cs
│   │   ├── OrderConfiguration.cs
│   │   ├── OrderItemConfiguration.cs
│   │   ├── ProductVariantConfiguration.cs
│   │   ├── ProductImageConfiguration.cs
│   │   ├── InventoryMovementConfiguration.cs
│   │   ├── WarehouseConfiguration.cs
│   │   ├── AddressConfiguration.cs
│   │   ├── PaymentConfiguration.cs
│   │   ├── InvoiceConfiguration.cs
│   │   └── ReviewConfiguration.cs
│   │
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   │
│   └── Repositories/
│       ├── ProductRepository.cs
│       ├── CategoryRepository.cs
│       ├── UserRepository.cs
│       ├── OrderRepository.cs
│       ├── CustomerRepository.cs
│       ├── AddressRepository.cs
│       ├── PaymentRepository.cs
│       ├── ReviewRepository.cs
│       ├── InvoiceRepository.cs
│       ├── InventoryRepository.cs
│       ├── RoleRepository.cs
│       └── UnitOfWork.cs
│
└── BusinessCore.API/                                  [PRESENTATION LAYER]
    ├── Controllers/
    │   ├── ProductsController.cs
    │   ├── CategoriesController.cs
    │   ├── BrandsController.cs
    │   ├── UsersController.cs
    │   ├── OrdersController.cs
    │   ├── CustomersController.cs
    │   ├── AddressesController.cs
    │   ├── PaymentsController.cs
    │   ├── ReviewsController.cs
    │   ├── InvoicesController.cs
    │   ├── InventoryController.cs
    │   ├── RolesController.cs
    │   └── DashboardController.cs
    │
    ├── Middleware/
    │   ├── GlobalExceptionMiddleware.cs
    │   ├── RequestLoggingMiddleware.cs
    │   ├── CorrelationIdMiddleware.cs
    │   ├── PerformanceMiddleware.cs
    │   └── SecurityHeadersMiddleware.cs
    │
    ├── Extensions/
    │   ├── ServiceExtensions.cs
    │   ├── SwaggerExtensions.cs
    │   ├── JwtExtensions.cs
    │   └── HealthCheckExtensions.cs
    │
    ├── Program.cs
    ├── appsettings.json
    └── appsettings.Development.json
✨ Características Principales
🔐 Autenticación y Autorización
✅ JWT con refresh tokens

✅ Roles: SuperAdmin, Admin, Manager, Employee, Customer

✅ Protección de endpoints por rol

✅ Políticas de autorización personalizadas

📦 Gestión de Productos
✅ CRUD completo

✅ Categorías y subcategorías

✅ Marcas y fabricantes

✅ Variantes de productos (talla, color, etc.)

✅ Imágenes de productos

✅ Stock y alertas de bajo stock

🛒 Gestión de Órdenes
✅ Creación de órdenes con múltiples items

✅ Estados: Pending, Processing, Shipped, Delivered, Cancelled, Returned

✅ Seguimiento de envíos

✅ Historial de órdenes

💳 Gestión de Pagos
✅ Múltiples métodos de pago

✅ Procesamiento de pagos

✅ Reembolsos

✅ Facturación

📊 Dashboard y Reportes
✅ Estadísticas en tiempo real

✅ Reportes de ventas

✅ Productos más vendidos

✅ Clientes top

✅ Alertas de stock

📝 Auditoría
✅ Registro de todas las acciones

✅ Trazabilidad completa

✅ Logs de errores

📡 Endpoints de la API
🔐 Autenticación (UsersController)
Método	Endpoint	Descripción	Autenticación
POST	/api/users/login	Iniciar sesión	❌
POST	/api/users/register	Registrar usuario	❌
POST	/api/users/refresh-token	Refrescar token	❌
POST	/api/users/logout	Cerrar sesión	✅
📦 Productos (ProductsController)
Método	Endpoint	Descripción	Autenticación
GET	/api/products	Obtener todos los productos	❌
GET	/api/products/active	Obtener productos activos	❌
GET	/api/products/{id}	Obtener producto por ID	❌
GET	/api/products/by-category/{categoryId}	Productos por categoría	❌
GET	/api/products/by-brand/{brandId}	Productos por marca	❌
GET	/api/products/search	Buscar productos	❌
GET	/api/products/low-stock	Productos con bajo stock	✅
GET	/api/products/paged	Productos paginados	❌
POST	/api/products	Crear producto	✅
PUT	/api/products/{id}	Actualizar producto	✅
DELETE	/api/products/{id}	Eliminar producto	✅
📋 Órdenes (OrdersController)
Método	Endpoint	Descripción	Autenticación
GET	/api/orders	Obtener todas las órdenes	✅
GET	/api/orders/{id}	Obtener orden por ID	✅
GET	/api/orders/user/{userId}	Órdenes de usuario	✅
GET	/api/orders/status/{status}	Órdenes por estado	✅
POST	/api/orders	Crear orden	✅
PATCH	/api/orders/{id}/status	Actualizar estado	✅
POST	/api/orders/{id}/cancel	Cancelar orden	✅
💳 Pagos (PaymentsController)
Método	Endpoint	Descripción	Autenticación
GET	/api/payments/{id}	Obtener pago	✅
GET	/api/payments/order/{orderId}	Pagos de orden	✅
POST	/api/payments/process	Procesar pago	✅
POST	/api/payments/{id}/refund	Reembolsar pago	✅
📊 Dashboard (DashboardController)
Método	Endpoint	Descripción	Autenticación
GET	/api/dashboard/stats	Estadísticas generales	✅
GET	/api/dashboard/top-products/{count}	Productos top	✅
GET	/api/dashboard/top-customers/{count}	Clientes top	✅
GET	/api/dashboard/monthly-sales/{year}	Ventas mensuales	✅
🚀 Instalación y Configuración
Requisitos Previos
.NET 8 SDK

SQL Server (o SQL Server LocalDB)

Visual Studio 2022 o VS Code

Pasos de Instalación
1. Clonar el Repositorio
bash
git clone https://github.com/tu-usuario/businesscore-api.git
cd businesscore-api
2. Configurar la Base de Datos
Actualizar la cadena de conexión en appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BusinessCoreDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
3. Restaurar Paquetes
bash
dotnet restore
4. Crear y Aplicar Migraciones
bash
# Crear migración inicial
dotnet ef migrations add InitialCreate --project BusinessCore.Infrastructure --startup-project BusinessCore.API

# Aplicar migración a la base de datos
dotnet ef database update --project BusinessCore.Infrastructure --startup-project BusinessCore.API
5. Ejecutar la API
bash
dotnet run --project BusinessCore.API
6. Acceder a Swagger
Desarrollo: http://localhost:5001/swagger

Producción: https://tudominio.com/swagger

🎯 Uso de la API
Ejemplo: Login
http
POST /api/users/login
Content-Type: application/json

{
    "email": "admin@businesscore.com",
    "password": "Admin123!"
}
Respuesta:

json
{
    "success": true,
    "message": "Login exitoso",
    "data": {
        "user": {
            "id": 1,
            "email": "admin@businesscore.com",
            "firstName": "Admin",
            "lastName": "Principal",
            "roles": ["SuperAdmin", "Admin"]
        },
        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "refreshToken": "dGVzdF9yZWZyZXNoX3Rva2VuXzEyMzQ1...",
        "expiresAt": "2026-07-08T14:30:00Z"
    }
}
Ejemplo: Crear Producto
http
POST /api/products
Authorization: Bearer {accessToken}
Content-Type: application/json

{
    "name": "Laptop Gamer",
    "description": "Laptop de alto rendimiento para gaming",
    "sku": "LAP-001",
    "price": 1500.00,
    "costPrice": 1200.00,
    "stock": 10,
    "categoryId": 1,
    "brandId": 2,
    "isFeatured": true
}
Ejemplo: Paginación
http
GET /api/products/paged?pageNumber=1&pageSize=10&sortBy=price&sortAscending=true
📚 Documentación Swagger
La documentación completa de la API está disponible en Swagger UI:

URL Local: http://localhost:5001/swagger

URL Producción: https://tudominio.com/swagger

Características de Swagger:
✅ Documentación interactiva

✅ Prueba de endpoints desde el navegador

✅ Autenticación JWT integrada

✅ Modelos de datos detallados

Cómo usar Swagger con JWT:
Abrir http://localhost:5001/swagger

Hacer clic en "Authorize" (🔒)

Ingresar token: Bearer {token}

Probar endpoints autenticados

🔒 Seguridad
JWT Authentication
csharp
// Configuración en appsettings.json
"Jwt": {
    "Secret": "tu_super_secret_key_muy_larga_aqui_1234567890",
    "Issuer": "BusinessCore",
    "Audience": "BusinessCore",
    "ExpirationInHours": 24
}
Headers de Seguridad
http
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Content-Security-Policy: default-src 'self'
Referrer-Policy: strict-origin-when-cross-origin
Roles y Permisos
Rol	Permisos
SuperAdmin	Acceso total a toda la API
Admin	Gestión de usuarios, productos, órdenes
Manager	Gestión de productos y órdenes
Employee	Consulta de productos y órdenes
Customer	Solo sus propias órdenes y perfil
