using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.ViewComponents._ManagerLayoutViewComponents
{
    public class _ManagerLayoutHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
