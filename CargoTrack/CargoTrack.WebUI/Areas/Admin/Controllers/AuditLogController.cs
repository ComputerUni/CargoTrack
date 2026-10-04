using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class AuditLogController(IAuditLogService _auditLogService) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var logs = await _auditLogService.GetAllAsync();
            var pagedList = logs.ToPagedList(page, 10);
            return View(pagedList);
        }
    }
}
