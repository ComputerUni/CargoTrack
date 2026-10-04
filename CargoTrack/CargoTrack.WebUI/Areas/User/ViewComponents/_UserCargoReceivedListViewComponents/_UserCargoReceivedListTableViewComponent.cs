using CargoTrack.DTO.DTOs.CargosDtos;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoReceivedListViewComponents
{
    public class _UserCargoReceivedListTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<ResultCargoDto> cargos, int page = 1)
        {
            var pagedList = cargos.ToPagedList(page, 5);
            return View(pagedList);
        }
    }
}
