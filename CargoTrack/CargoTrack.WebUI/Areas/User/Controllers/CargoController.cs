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
            var cargos = await _userCargoService.GetFilteredActiveUserCargosAsync(user.Id, search, status, dateRange);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.DateRange = dateRange;
            return View(cargos);
        }

        public async Task<IActionResult> SentList(string search, string status, string dateRange, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetFilteredSentUserCargosAsync(user.Id, search, status, dateRange);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.DateRange = dateRange;
            return View(cargos);
        }

        public async Task<IActionResult> ReceivedList(string search, string status, string dateRange, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetFilteredReceivedUserCargosAsync(user.Id, search, status, dateRange);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.DateRange = dateRange;
            return View(cargos);
        }

        public async Task<IActionResult> DeliveredList(string search, string status, string dateRange, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _userCargoService.GetFilteredDeliveredUserCargosAsync(user.Id, search, status, dateRange);
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.DateRange = dateRange;
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
