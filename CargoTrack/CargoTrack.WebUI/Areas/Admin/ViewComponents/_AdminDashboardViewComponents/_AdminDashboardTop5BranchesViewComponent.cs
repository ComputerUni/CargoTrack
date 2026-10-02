using CargoTrack.Business.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardTop5BranchesViewComponent(IStatisticsService _statisticService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var branches = await _statisticService.GetTopBranchAsync();
            return View(branches);
        }
    }
}
