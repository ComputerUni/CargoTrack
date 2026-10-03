using CargoTrack.Business.Services.Branches;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    [Authorize(Roles = $"{Roles.Manager}, {Roles.Admin}")]
    public class DashboardController(UserManager<AppUser> _userManager, IBranchService _branchService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            return View();
        }
    }
}
