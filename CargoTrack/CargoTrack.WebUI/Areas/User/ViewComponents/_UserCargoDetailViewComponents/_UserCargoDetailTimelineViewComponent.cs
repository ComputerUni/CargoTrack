using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoDetailViewComponents
{
    public class _UserCargoDetailTimelineViewComponent(UserManager<AppUser> _userManager, IUserCargoService _userCargoService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Guid cargoId)
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var cargo = await _userCargoService.GetByIdAsync(user.Id, cargoId);
            ViewBag.MovementCount = cargo?.CargoMovements?.Count ?? 0;
            return View(cargo);
        }
    }
}
