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
using CargoTrack.WebUI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    public class CargoController(IDeliveryService _deliveryService, ICargoService _cargoService, UserManager<AppUser> _userManager, IBranchService _branchService, IEmployeeService _employeeService, ITransferCenterService _transferCenterService) : Controller
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

            var users = await _userManager.GetUsersInRoleAsync("User");
            ViewBag.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }).ToList();

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


            var user = await _userManager.GetUserAsync(User);
            var userBranch = await _branchService.GetByIdAsync(user.BranchId.Value);
            var branch = await _branchService.GetAllAsync();
            var filteredBranches = branch
                        .Where(x => x.Id != user.BranchId.Value)
                        .Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = $"{x.Name} ({x.City?.Name})" 
                        })
                        .OrderBy(x => x.Text)
                        .ToList();
            ViewBag.Branches = filteredBranches;

            var employees = await _employeeService.GetByBranchIdAsync(user.BranchId.Value);
            ViewBag.Employees = employees.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }).ToList();

            var transferCenter = await _transferCenterService.GetAllAsync();
            ViewBag.TransferCenter = new SelectList(transferCenter, "Id", "Name");

            ViewBag.OriginalName = userBranch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
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

        public async Task<IActionResult> CargoDetail(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OutgoingName = branch.Name;
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            var cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            return View(cargo);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCargo()
        {
            var user = await _userManager.GetUserAsync(User);
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            await GetViewBagDataAsync();
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;
            ViewBag.BranchId = user.BranchId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCargo(CreateCargoDto createCargoDto)
        {
            if (!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                return View(createCargoDto);
            }

            var user = await _userManager.GetUserAsync(User);
            ViewBag.BranchId = user.BranchId;
            createCargoDto.OriginBranchId = user.BranchId.Value;
            var branch = await _branchService.GetByIdAsync(user.BranchId.Value);
            ViewBag.OriginalName = branch.Name;
            ViewBag.Manager = user.FirstName + " " + user.LastName;

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out var currentUserId))
            {
                createCargoDto.CurrentUserId = currentUserId;
            }

            await _cargoService.CreateAsync(createCargoDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCargo(Guid id)
        {
            await GetViewBagDataAsync();
            var cargo = await _cargoService.GetByIdAsync(id);
            return View(cargo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCargo(UpdateCargoDto updateCargoDto)
        {
            if (!ModelState.IsValid)
            {
                await GetViewBagDataAsync();
                return View(updateCargoDto);
            }

            await _cargoService.UpdateAsync(updateCargoDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCargo(Guid id)
        {
            await _cargoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadCargoSlip(Guid id)
        {
            var cargo = await _cargoService.GetByIdWithDetailsAsync(id);
            var pdfBytes = CargoPdfGenerator.Generate(cargo);
            return File(pdfBytes, "application/json", $"kargo-{cargo.TrackCode}.pdf");
        }
    }
}
