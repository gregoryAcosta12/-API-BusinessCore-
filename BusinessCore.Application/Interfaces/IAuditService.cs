using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Audit;
using BusinessCore.Application.DTOs.Common;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de auditoría
    /// Define las operaciones para el registro de auditoría
    /// </summary>
    public interface IAuditService
    {
        // Registro
        Task LogAsync(AuditLogDto auditLog);
        Task LogActionAsync(string userId, string action, string entity, string entityId, object details);
        Task LogErrorAsync(string userId, string action, string errorMessage, string stackTrace);

        // Consultas
        Task<IEnumerable<AuditLogDto>> GetUserLogsAsync(string userId);
        Task<IEnumerable<AuditLogDto>> GetEntityLogsAsync(string entity, string entityId);
        Task<IEnumerable<AuditLogDto>> GetActionLogsAsync(string action);
        Task<IEnumerable<AuditLogDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<PagedResultDto<AuditLogDto>> GetPagedAsync(AuditFilterDto filter);

        // Estadísticas
        Task<AuditStatisticsDto> GetAuditStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ActivitySummaryDto>> GetUserActivitySummaryAsync();
    }

    public class AuditLogDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public object Details { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AuditFilterDto
    {
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsSuccess { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class AuditStatisticsDto
    {
        public int TotalLogs { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public Dictionary<string, int> Actions { get; set; }
        public Dictionary<string, int> Entities { get; set; }
        public Dictionary<string, int> Users { get; set; }
    }

    public class ActivitySummaryDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int ActivityCount { get; set; }
        public DateTime LastActivity { get; set; }
        public Dictionary<string, int> Actions { get; set; }
    }
}