using CargoTrack.Business.Services.ManagerDashboard;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.ViewComponents._ManagerDashboardViewComponents
{
    public class _ManagerDashboardCourierPerformanceViewComponent(IManagerDashboardService _managerDashboardService, UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var courier = await _managerDashboardService.GetEmployeeDeliveryPerformance(user.BranchId.Value);
            return View(courier);
        }
    }
}
