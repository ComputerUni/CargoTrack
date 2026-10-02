using CargoTrack.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminLayoutViewComponents
{
    public class _AdminLayoutSidebarViewComponent(UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            var fullName = $"{user.FirstName} {user.LastName}";
            ViewBag.FullName = fullName;
            var role = await _userManager.GetRolesAsync(user);
            ViewBag.Role = role.FirstOrDefault();
            return View();
        }
    }
}
