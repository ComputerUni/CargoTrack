using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class CargoController(ICargoService _cargoService, IBranchService _branchService, UserManager<AppUser> _userManager) : Controller
    {
        private async Task GetViewBagDataAsync()
        {
            ViewBag.CargoTypes = Enum.GetValues(typeof(CargoType))
               .Cast<CargoType>()
               .Select(x => new SelectListItem
               {
                   Text = x.GetDisplayName(),
                   Value = ((int)x).ToString()
               }).ToList();

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }).ToList();

            var branches = await _branchService.GetAllAsync();
            ViewBag.Branches = new SelectList(branches, "Id", "Name");
        }


        public async Task<IActionResult> Index()
        {
            var cargos = await _cargoService.GetAllAsync();
            return View(cargos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetViewBagDataAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoDto createCargoDto)
        {
            if(!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                return View(createCargoDto);
            }

            await _cargoService.CreateAsync(createCargoDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetViewBagDataAsync();
            var cargo = await _cargoService.GetByIdAsync(id);
            return View(cargo);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCargoDto updateCargoDto)
        {
            if(!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                return View(updateCargoDto);
            }

            await _cargoService.UpdateAsync(updateCargoDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _cargoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
