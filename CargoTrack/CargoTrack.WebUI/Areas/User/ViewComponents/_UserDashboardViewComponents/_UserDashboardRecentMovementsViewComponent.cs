using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserDashboardViewComponents
{
    public class _UserDashboardRecentMovementsViewComponent(UserManager<AppUser> _userManager, IUserCargoService _userCargoService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var activeCargos = await _userCargoService.GetByUserIdAsync(user.Id);
            var cargoMovements = activeCargos.Where(x => x.SenderId == user.Id)
                                             .OrderByDescending(x => x.CargoMovements)
                                             .Take(3)
                                             .ToList();   
            return View(cargoMovements);
        }
    }
}
