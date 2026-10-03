using CargoTrack.Business.Services.PerformanceReports;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminReportViewComponents
{
    public class _AdminReportCourierPerformanceViewComponent(IPerformanceReportService _performanceReportService) : ViewComponent 
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var couriers = await _performanceReportService.GetCourierPerformancesAsync();
            return View(couriers);
        }
    }
}
