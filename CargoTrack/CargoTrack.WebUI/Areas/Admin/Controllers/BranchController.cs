using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cities;
using CargoTrack.DTO.DTOs.BranchDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class BranchController(IBranchService _branchService, ICityService _cityService) : Controller
    {
        private async Task GetCitiesAsync()
        {
            var cities = await _cityService.GetAllAsync();
            var sortedCities = cities.OrderBy(x => x.Name).ToList();
            ViewBag.cities = (from city in sortedCities
                              select new SelectListItem
                              {
                                  Text = city.Name,
                                  Value = city.Id.ToString()
                              }).ToList();
        }

        public async Task<IActionResult> Index(string? search, string? city)
        {
            var branches = await _branchService.GetFilteredBranchesAsync(search, city);
            var allBranches = await _branchService.GetAllAsync();
            ViewBag.Cities = allBranches.Where(x => !string.IsNullOrEmpty(x.City.Name)).Select(x => x.City.Name).Distinct().OrderBy(x => x).ToList();
            ViewBag.Search = search;
            ViewBag.SelectedCity = city;
            return View(branches);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBranchDto createBranchDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(createBranchDto);
            }

            await _branchService.CreateAsync(createBranchDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetCitiesAsync();
            var branch = await _branchService.GetByIdAsync(id);
            return View(branch);
        }

        [HttpPost] 
        public async Task<IActionResult> Update(UpdateBranchDto updateBranchDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(updateBranchDto);
            }

            await _branchService.UpdateAsync(updateBranchDto);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            await _branchService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));

        }

    }
}
