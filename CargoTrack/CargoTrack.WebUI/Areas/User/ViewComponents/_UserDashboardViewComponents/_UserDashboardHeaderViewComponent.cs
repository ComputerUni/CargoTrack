using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserDashboardViewComponents
{
    public class _UserDashboardHeaderViewComponent(UserManager<AppUser> _userManager, IUserCargoService _userCargoService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var role = await _userManager.GetRolesAsync(user);
            ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            ViewBag.Role = role.FirstOrDefault();

            var activeCargos = await _userCargoService.GetByUserIdAsync(user.Id);
            ViewBag.ActiveCargoCount = activeCargos.Count;

            var nearestCargo = activeCargos.Where(x => x.ReceiverId == user.Id)
                                           .OrderBy(x => x.EstimatedArrivalDate)
                                           .FirstOrDefault();

            if(nearestCargo != null)
            {
                ViewBag.HasIncoming = true;
                ViewBag.EstimatedDate = nearestCargo.EstimatedArrivalDate.ToString("dd MMMM yyyy, HH:mm");
            }

            else
            {
                ViewBag.HasIncoming = false;
            }

            return View();
        }
    }
}
