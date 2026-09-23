using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Areas.Manager.Models;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    public class CargoController(ICargoService _cargoService, UserManager<AppUser> _userManager, IBranchService _branchService, IEmployeeService _employeeService, ITransferCenterService _transferCenterService) : Controller
    {

        private async Task GetViewBagDataAsync()
        {
            ViewBag.CargoStatus = Enum.GetValues(typeof(CargoStatus))
               .Cast<CargoStatus>()
               .Select(x => new SelectListItem
               {
                   Text = x.GetDisplayName(),
                   Value = ((int)x).ToString()
               }).ToList();

            var branches = await _branchService.GetAllAsync();
            ViewBag.Branches = new SelectList(branches, "Id", "Name");

            var user = await _userManager.GetUserAsync(User);
            var employees = await _employeeService.GetByBranchIdAsync(user.BranchId.Value);
            ViewBag.Employees = employees.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }).ToList();

            var transferCenter = await _transferCenterService.GetAllAsync();
            ViewBag.TransferCenter = new SelectList(transferCenter, "Id", "Name");
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _cargoService.GetByBranchIdAsync(user.BranchId.Value);
            return View(cargos);
        }

        [HttpGet]
        public async Task<IActionResult> AddMovement(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await GetViewBagDataAsync();
            var vm = new CargoMovementViewModel
            {
                Cargo = await _cargoService.GetByIdWithDetailsAsync(id),
                StatusUpdate = new CargoStatusUpdateDto { 
                    Id = id,
                    BranchId = user.BranchId.Value
                }
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovement(CargoMovementViewModel vm)
        {
            ModelState.Remove("Cargo");

            if (!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.StatusUpdate.Id);
                return View(vm);
            }

            await _cargoService.UpdateStatusAsync(vm.StatusUpdate);
            return RedirectToAction("Index");
        }
    }
}
