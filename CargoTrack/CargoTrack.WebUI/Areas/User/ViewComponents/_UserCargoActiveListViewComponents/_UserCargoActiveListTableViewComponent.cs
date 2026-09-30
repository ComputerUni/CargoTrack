using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoActiveListViewComponents
{
    public class _UserCargoActiveListTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ResultCargoDto> cargos)
        {
            return View(cargos);
        }
    }
}
