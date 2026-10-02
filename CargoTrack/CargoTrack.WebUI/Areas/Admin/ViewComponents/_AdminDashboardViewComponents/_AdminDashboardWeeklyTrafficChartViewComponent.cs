using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardWeeklyTrafficChartViewComponent(IStatisticsService _statisticService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var traffic = await _statisticService.GetWeeklyTrafficAsync();
            return View(traffic);
        }
    }
}
