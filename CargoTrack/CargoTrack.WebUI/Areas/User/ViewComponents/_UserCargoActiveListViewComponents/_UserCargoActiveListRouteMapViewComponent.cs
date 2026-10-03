using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoActiveListViewComponents
{
    public class _UserCargoActiveListRouteMapViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ResultCargoDto cargo)
        {
            if(cargo is null)
            {
                return Content(string.Empty);
            }
            return View(cargo);
        }
    }
}
