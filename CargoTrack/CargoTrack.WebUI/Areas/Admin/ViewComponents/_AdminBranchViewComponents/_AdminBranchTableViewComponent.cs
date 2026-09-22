using CargoTrack.DTO.DTOs.BranchDtos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminBranchViewComponents
{
    public class _AdminBranchTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<ResultBranchDto> branches)
        {
            return View(branches);
        }
    }
}
