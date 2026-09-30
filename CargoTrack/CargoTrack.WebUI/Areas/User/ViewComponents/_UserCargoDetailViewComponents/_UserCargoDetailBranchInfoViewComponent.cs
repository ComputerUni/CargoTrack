using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.UserCargos;
using CargoTrack.DTO.DTOs.BranchDtos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserCargoDetailViewComponents
{
    public class _UserCargoDetailBranchInfoViewComponent(IUserCargoService _userCargoService, UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ResultCargoDto cargo)
        {
            return View(cargo);
        }
    }
}
