using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.Controllers
{
    [Area(Area.User)]
    public class CargoController(IUserCargoService _userCargoService, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> ActiveList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetByUserIdAsync(user.Id);
            return View(cargos);
        }

        public async Task<IActionResult> SentList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetSentByUserIdAsync(user.Id);
            return View(cargos);
        }

        public async Task<IActionResult> ReceivedList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetReceivedByUserIdAsync(user.Id);
            return View(cargos);
        }

        public async Task<IActionResult> DeliveredList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetDeliveredByUserIdAsync(user.Id);
            return View(cargos);
        }

        public async Task<IActionResult> CargoDetail(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var cargo = await _userCargoService.GetByIdAsync(user.Id, id);
            if(cargo == null)
            {
                return RedirectToAction("ActiveList");
            }
            return View(cargo);
        }
    }
}
