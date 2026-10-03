using CargoTrack.Business.Services.PerformanceReports;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminReportViewComponents
{
    public class _AdminReportExceptionReasonAnalysisViewComponent(IPerformanceReportService _performanceReportService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var exceptions = await _performanceReportService.GetDeliveryExceptionStatusesAsync();
            return View(exceptions);
        }
    }
}
