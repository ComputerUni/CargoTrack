using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserDashboardViewComponents
{
    public class _UserDashboardFeaturedShipmentViewComponent(UserManager<AppUser> _userManager, IUserCargoService _userCargoService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var activeCargos = await _userCargoService.GetByUserIdAsync(user.Id);
            var lastCargo = activeCargos.OrderByDescending(x => x.CargoStatus == CargoStatus.OutForDelivery).ThenByDescending(x => x.CreatedDate).FirstOrDefault();
            return View(lastCargo);
        }
    }
}
