using CargoTrack.Business.Services.ManagerDashboard;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.ViewComponents._ManagerDashboardViewComponents
{
    public class _ManagerDashboardSlaAlertsViewComponent(IManagerDashboardService _managerDashboardService, UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var movements = await _managerDashboardService.GetFailedCargoMovementsAsync(user.BranchId.Value);
            return View(movements);
        }
    }
}
