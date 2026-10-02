using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardStatusDonutViewComponent(IStatisticsService _statisticService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var donut = await _statisticService.GetStatusDonutAsync();
            return View(donut);
        }
    }
}
