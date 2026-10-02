using CargoTrack.Business.Services.Cargos;
using CargoTrack.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Controllers
{
    public class DefaultController(ICargoService _cargoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
          
            return View();
        }

        public async Task<IActionResult> CargoDetails(string trackCode)
        {
            var cargo = await _cargoService.GetByTrackCodeAsync(trackCode);
            if(cargo is null)
            {
                TempData["error"] = "Bu takip numarasına ait bir kargo bulunamadı";
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }
    }
}
