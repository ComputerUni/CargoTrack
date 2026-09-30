using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoDetailViewComponents
{
    public class _UserCargoDetailHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ResultCargoDto cargo)
        {
            return View(cargo);
        }
    }
}
