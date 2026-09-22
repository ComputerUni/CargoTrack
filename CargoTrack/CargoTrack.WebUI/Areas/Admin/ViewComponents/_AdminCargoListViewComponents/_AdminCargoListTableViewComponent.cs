using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminCargoListViewComponents
{
    public class _AdminCargoListTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ResultCargoDto> cargos)
        {
            return View(cargos);
        }
    }
}
