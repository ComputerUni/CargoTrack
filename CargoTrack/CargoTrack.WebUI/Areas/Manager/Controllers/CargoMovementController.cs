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

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    public class CargoMovementController(UserManager<AppUser> _userManager, IBranchService _branchService, ICargoService _cargoService, IEmployeeService _employeeService, ITransferCenterService _transferCenterService) : Controller
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

            ViewBag.ExceptionReason = Enum.GetValues(typeof(ExceptionReason))
                .Cast<ExceptionReason>()
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

        [HttpGet]
        public async Task<IActionResult> AddMovement(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            await GetViewBagDataAsync();

            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.CurrentBranchName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;

            var cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            var lastMovement = cargo.CargoMovements?.OrderByDescending(m => m.MovementDate).FirstOrDefault();
            bool isComing = cargo.CargoMovements != null && cargo.CargoMovements.Any() ? lastMovement?.BranchId != user.BranchId : cargo.OriginBranchId != user.BranchId;
            if (isComing)
            {
                ViewBag.CargoStatus = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Varış Şubesine Ulaştı",
                        Value = ((int)CargoStatus.ArrivedAtDeliveryBranch).ToString()
                    }
                };
            }

            var vm = new CargoMovementViewModel
            {
                Cargo = await _cargoService.GetByIdWithDetailsAsync(id),
                StatusUpdate = new CargoStatusUpdateDto
                {
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

            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            vm.StatusUpdate.BranchId = user.BranchId.Value;

            if (!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.StatusUpdate.Id);
                return View(vm);
            }

            try
            {
                await _cargoService.UpdateStatusAsync(vm.StatusUpdate);
                return RedirectToAction("Index", "Cargo");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await GetViewBagDataAsync();
                ViewBag.CurrentBranchName = branch.Name;
                ViewBag.OriginalName = branch.Name;
                ViewBag.Manager = user.FirstName + " " + user.LastName;
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.StatusUpdate.Id);
                return View(vm);
            }
        }
    }
}
