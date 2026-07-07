using BusinessCore.API.Extensions;
using BusinessCore.API.Middleware;
using BusinessCore.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. CONFIGURACIÓN DE SERVICIOS
// ============================================

// Agregar servicios de la aplicación (DbContext, Repositorios, Servicios, AutoMapper, FluentValidation)
builder.Services.AddApplicationServices(builder.Configuration);

// Configurar Swagger/OpenAPI para la documentación
builder.Services.AddSwaggerConfiguration();

// Agregar autenticación JWT para proteger los endpoints
builder.Services.AddJwtAuthentication(builder.Configuration);

// Agregar controladores con configuración JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configurar CORS para permitir peticiones desde otros dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:4200", "https://localhost:3000", "http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// Agregar Health Checks para monitoreo del estado de la aplicación
builder.Services.AddCustomHealthChecks();

// Configurar compresión de respuestas para mejorar rendimiento
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// ============================================
// 2. CONSTRUCCIÓN DE LA APLICACIÓN
// ============================================

var app = builder.Build();

// ============================================
// 3. PIPELINE DE MIDDLEWARES (ORDEN IMPORTANTE)
// ============================================

// Correlation ID para rastrear peticiones de principio a fin
app.UseMiddleware<CorrelationIdMiddleware>();

// Headers de seguridad para proteger la aplicación
app.UseMiddleware<SecurityHeadersMiddleware>();

// Logging de todas las peticiones entrantes
app.UseMiddleware<RequestLoggingMiddleware>();

// Monitoreo de rendimiento para detectar peticiones lentas
app.UseMiddleware<PerformanceMiddleware>();

// Swagger solo disponible en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

// Comprimir respuestas para mejorar rendimiento
app.UseResponseCompression();

// Redirigir HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar CORS (siempre antes de autenticación)
app.UseCors("AllowAll");

// Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// Manejo global de excepciones (¡ÚLTIMO! captura errores de todo lo anterior)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Mapear endpoints de la API
app.MapControllers();

// Endpoint para Health Checks
app.MapHealthChecks("/health");

// ============================================
// 4. MIGRACIONES AUTOMÁTICAS
// ============================================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // En desarrollo, aplicar migraciones automáticamente
        dbContext.Database.Migrate();
    }
    else
    {
        // En producción, solo asegurar que la base de datos existe
        dbContext.Database.EnsureCreated();
    }
}

// ============================================
// 5. EJECUTAR LA APLICACIÓN
// ============================================

app.Run();