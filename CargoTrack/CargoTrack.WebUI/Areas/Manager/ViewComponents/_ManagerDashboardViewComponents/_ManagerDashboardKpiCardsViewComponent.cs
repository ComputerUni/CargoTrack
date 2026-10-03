using CargoTrack.Business.Services.ManagerDashboard;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.ViewComponents._ManagerDashboardViewComponents
{
    public class _ManagerDashboardKpiCardsViewComponent(IManagerDashboardService _managerDashboardService, UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var kpi = await _managerDashboardService.GetKpiAsync(user.BranchId.Value);
            return View(kpi);
        }
    }
}
