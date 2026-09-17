using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.AuditLogs
{
    public class AuditLogService(IAuditLogRepository _auditLogRepository, UserManager<AppUser> _userManager) : IAuditLogService
    {
        public async Task CreateAuditLogAsync(Guid userId, Guid entityId , string actionType, string entityName, string description, string? oldValue = null, string newValue = null)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new KeyNotFoundException($"Audit log oluşturulamadı: '{userId}' ID'sine sahip kullanıcı bulunamadı.");
            }

            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ActionType = actionType,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                OldValue = oldValue,
                NewValue = newValue
            };

            await _auditLogRepository.CreateAsync(auditLog);
        }
    }
}
