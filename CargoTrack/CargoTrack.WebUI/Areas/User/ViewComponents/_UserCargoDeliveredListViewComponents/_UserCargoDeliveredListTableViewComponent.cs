using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoDeliveredListViewComponents
{
    public class _UserCargoDeliveredListTableViewComponent : ViewComponent
    { 
        public async Task<IViewComponentResult> InvokeAsync(List<ResultCargoDto> cargos)
        {
            return View(cargos);
        }
    }
}
