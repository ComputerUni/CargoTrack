using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoReceivedListViewComponents
{
    public class _UserCargoReceivedListTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<ResultCargoDto> cargos)
        {
            return View(cargos);
        }
    }
}
