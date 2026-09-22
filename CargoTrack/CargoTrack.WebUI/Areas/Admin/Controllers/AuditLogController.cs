using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class AuditLogController(IAuditLogService _auditLogService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var logs = await _auditLogService.GetAllAsync();
            return View();
        }
    }
}
