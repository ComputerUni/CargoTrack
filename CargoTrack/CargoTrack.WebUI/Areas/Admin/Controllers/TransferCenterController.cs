using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.TransferCenterDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class TransferCenterController(ITransferCenterService _transferCenterService, ICityService _cityService) : Controller
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

        public async Task<IActionResult> Index()
        {
            var transferCenters = await _transferCenterService.GetAllAsync();
            return View(transferCenters);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferCenterDto createTransferCenterDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(createTransferCenterDto);
            }

            await _transferCenterService.CreateAsync(createTransferCenterDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetCitiesAsync();
            var transferCenter = await _transferCenterService.GetByIdAsync(id);
            return View(transferCenter);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransferCenterDto updateTransferCenterDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(updateTransferCenterDto);
            }

            await _transferCenterService.UpdateAsync(updateTransferCenterDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _transferCenterService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
