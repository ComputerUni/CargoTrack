using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardSecondaryKpiViewComponent(IStatisticsService _statisticService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _statisticService.GetAdminDashboardSecondaryKpiAsync();
            return View(kpi);
        }
    }
}
