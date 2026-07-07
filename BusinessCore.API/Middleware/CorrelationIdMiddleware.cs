using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessCore.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Obtener CorrelationId del header o generar uno nuevo
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationIdHeader] = correlationId;
            }

            // Agregar al contexto para usar en logs
            context.Items["CorrelationId"] = correlationId.ToString();

            // Agregar al response header
            context.Response.Headers[CorrelationIdHeader] = correlationId.ToString();

            await _next(context);
        }
    }
}