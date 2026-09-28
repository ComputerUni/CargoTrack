using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.DeliveryDtos;
using CargoTrack.DTO.DTOs.ManagerCargoDtos;
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

            var vm = new CargoIndexViewModel
            {
                Cargos = await _cargoService.GetByBranchIdAsync(user.BranchId.Value),
                Summary = await _cargoService.GetBranchCargoSummaryAsync(user.BranchId.Value)
            };

            return View(vm);
        }

        public async Task<IActionResult> IncomingList()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.IncomingName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;

            var vm = new CargoIndexViewModel
            {
                Cargos = await _cargoService.GetIncomingCargosAsync(user.BranchId.Value),
                Incoming = await _cargoService.GetIncomingCargoSummaryAsync(user.BranchId.Value)
            };

            return View(vm);
        }

        public async Task<IActionResult> OutgoingList()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OutgoingName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;

            var vm = new CargoIndexViewModel
            {
                Cargos = await _cargoService.GetOutgoingCargosAsync(user.BranchId.Value),
                Outgoing = await _cargoService.GetOutgoingCargoSummaryAsync(user.BranchId.Value)
            };

            return View(vm);
        }
    }
}
