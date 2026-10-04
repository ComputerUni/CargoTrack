using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CargoTrack.WebUI.Areas.User.Controllers
{
    [Area(Area.User)]
    public class CargoController(IUserCargoService _userCargoService, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> ActiveList(string search, string status, string dateRange, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetFilteredUserCargosAsync(user.Id, search, status, dateRange, onlyActive: true);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.DateRange = dateRange;
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
            if (cargo == null)
            {
                return RedirectToAction("ActiveList");
            }
            return View(cargo);
        }

        public async Task<IActionResult> CargoDetailByCode(string trackCode)
        {
            if (string.IsNullOrEmpty(trackCode))
            {
                return RedirectToAction("Dashboard", "Index");
            }

            var user = await _userManager.GetUserAsync(User);
            var cargo = await _userCargoService.GetCargoDetailByCode(user.Id, trackCode);
            if (cargo == null)
            {
                return RedirectToAction("Dashboard", "Index");
            }

            return RedirectToAction("CargoDetail", new { id = cargo.Id });
        }
    }
}
