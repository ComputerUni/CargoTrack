using CargoTrack.Business.Services.PerformanceReports;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminReportViewComponents
{
    public class _AdminReportKpiSummaryViewComponent(IPerformanceReportService _performanceReportService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _performanceReportService.GetKpiSummaryAsync();
            return View(kpi);
        }
    }
}
