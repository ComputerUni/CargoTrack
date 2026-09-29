using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.Controllers
{
    [Area(Area.User)]
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
