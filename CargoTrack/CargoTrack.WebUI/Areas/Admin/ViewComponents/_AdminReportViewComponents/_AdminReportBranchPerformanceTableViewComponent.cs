using CargoTrack.Business.Services.PerformanceReports;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminReportViewComponents
{
    public class _AdminReportBranchPerformanceTableViewComponent(IPerformanceReportService _performanceReportService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var branches = await _performanceReportService.GetBranchPerformancesAsync();
            return View(branches);
        }
    }
}
