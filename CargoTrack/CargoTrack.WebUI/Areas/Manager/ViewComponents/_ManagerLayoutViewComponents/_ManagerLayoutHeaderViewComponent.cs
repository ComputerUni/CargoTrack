using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Manager.ViewComponents._ManagerLayoutViewComponents
{
    public class _ManagerLayoutHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
