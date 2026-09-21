using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Employees;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class EmployeeController(IEmployeeService _employeeService, IBranchService _branchService) : Controller
    {
        private async Task GetBranchesAsync()
        {
            var branches = await _branchService.GetAllAsync();
            ViewBag.branches = (from branch in branches
                                select new SelectListItem
                                {
                                    Text = branch.Name,
                                    Value = branch.Id.ToString()
                                }).ToList();
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetBranchesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto createEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                await GetBranchesAsync();
                return View(createEmployeeDto);
            }

            await _employeeService.CreateAsync(createEmployeeDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetBranchesAsync();
            var employee = await _employeeService.GetByIdAsync(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                await GetBranchesAsync();
                return View(updateEmployeeDto);
            }

            await _employeeService.UpdateAsync(updateEmployeeDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
