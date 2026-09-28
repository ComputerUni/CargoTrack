using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.DeliveryDtos;
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
    public class DeliveryController(UserManager<AppUser> _userManager, ICargoService _cargoService, IBranchService _branchService, IDeliveryService _deliveryService, IEmployeeService _employeeService, ITransferCenterService _transferCenterService) : Controller
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
        public async Task<IActionResult> VerifyDelivery(Guid id)
        {
            await GetViewBagDataAsync();
            var user = await _userManager.GetUserAsync(User);
            var cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            ViewBag.VerifyDelivery = branch.Name;

            if (string.IsNullOrEmpty(cargo.DeliveryCode))
            {
                await _deliveryService.GenerateDeliveryCodeAsync(id);
                cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            }

            var vm = new VerifyDeliveryViewModel
            {
                Cargo = await _cargoService.GetByIdWithDetailsAsync(id),
                DeliveryInput = new VerifyDeliveryDto { CargoId = id }
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyDelivery(VerifyDeliveryViewModel vm)
        {
            ModelState.Remove("Cargo");
            if (!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.DeliveryInput.CargoId);
                return View(vm);
            }

            await _deliveryService.VerifyAndCompleteDeliveryAsync(vm.DeliveryInput.CargoId, vm.DeliveryInput.DeliveryCode, vm.DeliveryInput.RecipientName, vm.DeliveryInput.EmployeeId, vm.DeliveryInput.Note);
            return RedirectToAction("Index", "Cargo");
        }

        public async Task<IActionResult> DeliveryList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _cargoService.GetOutDeliveryByBranchIdAsync(user.BranchId.Value);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            ViewBag.Delivery = branch.Name;
            return View(cargos);
        }

        public async Task<IActionResult> DeliveryFailedAndReturnInProcessList()
        {
            var user = await _userManager.GetUserAsync(User);
            var cargos = await _cargoService.GetDeliveryFailedOrReturnInProcessByBranchIdAsync(user.BranchId.Value);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            ViewBag.DeliveryFailed = branch.Name;
            return View(cargos);
        }
    }
}
