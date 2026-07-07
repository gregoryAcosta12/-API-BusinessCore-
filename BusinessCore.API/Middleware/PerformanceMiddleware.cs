using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessCore.API.Middleware
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;
        private const int SlowRequestThreshold = 5000; // 5 segundos

        public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                if (elapsedMilliseconds > SlowRequestThreshold)
                {
                    _logger.LogWarning(
                        "Slow Request - {Method} {Path} tardó {ElapsedMs}ms (umbral: {Threshold}ms)",
                        context.Request.Method,
                        context.Request.Path,
                        elapsedMilliseconds,
                        SlowRequestThreshold
                    );
                }
            }
        }
    }
}