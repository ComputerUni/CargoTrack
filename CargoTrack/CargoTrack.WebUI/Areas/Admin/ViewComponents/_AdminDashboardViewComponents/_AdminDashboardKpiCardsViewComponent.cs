using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardKpiCardsViewComponent(IStatisticsService _statisticsService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var summary = await _statisticsService.GetAdminDashboardSummaryAsync();
            return View(summary);
        }
    }
}
