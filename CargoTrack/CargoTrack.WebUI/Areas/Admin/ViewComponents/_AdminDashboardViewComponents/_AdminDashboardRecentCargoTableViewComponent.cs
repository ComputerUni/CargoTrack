using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardRecentCargoTableViewComponent(IStatisticsService _statisticsService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentMoves = await _statisticsService.GetRecentCargosAsync();
            return View(recentMoves);
        }
    }
}
