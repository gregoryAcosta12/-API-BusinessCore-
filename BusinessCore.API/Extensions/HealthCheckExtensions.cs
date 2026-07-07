using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BusinessCore.API.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<Infrastructure.Data.ApplicationDbContext>()
                .AddUrlGroup(new Uri("https://localhost:5001"), "API Health", HealthStatus.Unhealthy)
                .AddCheck<DatabaseHealthCheck>("Database");
        }
    }

    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly Infrastructure.Data.ApplicationDbContext _context;

        public DatabaseHealthCheck(Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                return canConnect
                    ? HealthCheckResult.Healthy("Database connection is healthy")
                    : HealthCheckResult.Unhealthy("Cannot connect to database");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database check failed", ex);
            }
        }
    }
}