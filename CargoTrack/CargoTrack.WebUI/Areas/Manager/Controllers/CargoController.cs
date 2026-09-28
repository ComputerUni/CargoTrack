using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.DeliveryDtos;
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
    public class CargoController(IDeliveryService _deliveryService, ICargoService _cargoService, UserManager<AppUser> _userManager, IBranchService _branchService, IEmployeeService _employeeService, ITransferCenterService _transferCenterService) : Controller
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

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            var cargos = await _cargoService.GetByBranchIdAsync(user.BranchId.Value);
            return View(cargos);
        }

        public async Task<IActionResult> IncomingList()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.IncomingName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            var cargos = await _cargoService.GetIncomingCargosAsync(user.BranchId.Value);
            return View(cargos);
        }

        public async Task<IActionResult> OutgoingList()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OutgoingName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            var cargos = await _cargoService.GetOutgoingCargosAsync(user.BranchId.Value);
            return View(cargos);
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
            if(isComing)
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
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                await GetViewBagDataAsync();
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.StatusUpdate.Id);
                return View(vm);
            }

           
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDelivery(Guid id)
        {
            await GetViewBagDataAsync();
            var user = await _userManager.GetUserAsync(User);
            var cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.VerifyDelivery = branch.Name;

            if (string.IsNullOrEmpty(cargo.DeliveryCode))
            {
                await _deliveryService.GenerateDeliveryCodeAsync(id);
                cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            }

            var vm = new VerifyDeliveryViewModel
            {
                Cargo = await _cargoService.GetByIdWithDetailsAsync(id),
                DeliveryInput = new VerifyDeliveryDto { CargoId = id}
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyDelivery(VerifyDeliveryViewModel vm)
        {
            ModelState.Remove("Cargo");
            if(!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                vm.Cargo = await _cargoService.GetByIdWithDetailsAsync(vm.DeliveryInput.CargoId);
                return View(vm);
            }

            await _deliveryService.VerifyAndCompleteDeliveryAsync(vm.DeliveryInput.CargoId, vm.DeliveryInput.DeliveryCode, vm.DeliveryInput.RecipientName, vm.DeliveryInput.EmployeeId, vm.DeliveryInput.Note);
            return RedirectToAction("Index");
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
