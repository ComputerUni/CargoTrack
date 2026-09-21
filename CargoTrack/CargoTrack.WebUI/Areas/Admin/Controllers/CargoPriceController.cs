using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    public class CargoPriceController(ICargoPricingService _cargoPricingService) : Controller
    {
        private async Task GetCargoTypes()
        {
            ViewBag.CargoTypes = Enum.GetValues(typeof(CargoType))
                .Cast<CargoType>()
                .Select(x => new SelectListItem
                {
                    Text = x.GetDisplayName(),
                    Value = ((int)x).ToString()
                }).ToList();
        }

        public async Task<IActionResult> Index()
        {
            var pricings = await _cargoPricingService.GetAllAsync();
            return View(pricings);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCargoTypes();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoPriceDto createCargoPriceDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCargoTypes();
                return View(createCargoPriceDto);
            }

            await _cargoPricingService.CreateAsync(createCargoPriceDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetCargoTypes();
            var cargoPrice = await _cargoPricingService.GetByIdAsync(id);
            return View(cargoPrice);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCargoPriceDto updateCargoPriceDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCargoTypes();
                return View(updateCargoPriceDto);
            }

            await _cargoPricingService.UpdateAsync(updateCargoPriceDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _cargoPricingService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
