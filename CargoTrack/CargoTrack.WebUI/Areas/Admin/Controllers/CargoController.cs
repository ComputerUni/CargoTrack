using CargoTrack.Business.Services.Cargos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class CargoController(ICargoService _cargoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var cargos = await _cargoService.GetAllAsync();
            return View(cargos);
        }
    }
}
