using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminCargoListViewComponents
{
    public class _AdminCargoListTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ResultCargoDto> cargos, int page = 1)
        {
            var pagedList = cargos.ToPagedList(page, 12);
            return View(pagedList);
        }
    }
}
