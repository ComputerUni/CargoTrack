using CargoTrack.Business.Services.Cargos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    public class CargoController(ICargoService _cargoService, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _cargoService.GetByBranchIdAsync(user.BranchId.Value);
            return View(cargos);
        }
    }
}
