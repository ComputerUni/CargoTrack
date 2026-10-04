using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoDeliveredListViewComponents
{
    public class _UserCargoDeliveredListTableViewComponent : ViewComponent
    { 
        public async Task<IViewComponentResult> InvokeAsync(List<ResultCargoDto> cargos, int page = 1)
        {
            var pagedList = cargos.ToPagedList(page, 5);
            return View(pagedList);
        }
    }
}
