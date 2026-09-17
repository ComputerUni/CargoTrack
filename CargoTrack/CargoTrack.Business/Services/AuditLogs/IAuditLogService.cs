using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.AuditLogs
{
    public interface IAuditLogService
    {
        Task CreateAuditLogAsync(Guid userId, Guid entityId, string actionType, string entityName, string description, string? oldValue = null, string? newValue = null);
    }
}
