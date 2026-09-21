using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.CargoPricings
{
    public class CargoPricingService(ICargoPricesRepository _repository, IBranchRepository _branchRepository) : ICargoPricingService
    {
        public async Task<DateTime> CalculateEstimatedDeliveryDateAsync(Guid originalBranchId, Guid destinationBranchId, CargoType cargoType)
        {
            var originBranch = await _branchRepository.GetByIdAsync(originalBranchId);
            var destinationBranch = await _branchRepository.GetByIdAsync(destinationBranchId);

            var isIntercity = originBranch.CityId != destinationBranch.CityId;

            var days = (cargoType, isIntercity) switch
            {
                (CargoType.Urgent, false) => 0,
                (CargoType.Urgent, true) => 1,
                (CargoType.Standart, false) => 1,
                (CargoType.Standart, true) => 3,
                (CargoType.Fragile, _) => 4,
                (CargoType.Heavy, _) => 4,
                _ => 3
            };

            return DateTime.Now.AddDays(days);
        }

        public async Task<decimal> CalculatePriceAsync(double weight, double desi, CargoType cargoType, bool isIntercity)
        {
            var priceRule = await _repository.GetMatchingRuleAsync(weight, cargoType, isIntercity);
            var effectiveWeight = (decimal)Math.Max(weight, desi);
            var price = priceRule.BasePrice + (effectiveWeight * (decimal)priceRule.DesiCoefficient) + priceRule.AdditionalServicePrice;
            return price;

        }

        public async Task CreateAsync(CreateCargoPriceDto createCargoPriceDto)
        {
            var cargoPrice = createCargoPriceDto.Adapt<CargoPrice>();
            await _repository.CreateAsync(cargoPrice);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cargoPrice = await _repository.GetByIdAsync(id);

            if (cargoPrice is null)
            {
                throw new ValidationException("Cargo Price Not Found");
            }

            await _repository.DeleteAsync(cargoPrice);
        }

        public async Task<string> GenerateTrackCode()
        {
            var year = DateTime.Now.Year;
            var random = new Random().Next(100000, 999999);
            return $"CT-{year}-{random}";
        }

        public async Task<List<ResultCargoPriceDto>> GetAllAsync()
        {
            var cargoPrices = await _repository.GetAllAsync();
            return cargoPrices.Adapt<List<ResultCargoPriceDto>>();
        }

        public async Task<UpdateCargoPriceDto> GetByIdAsync(Guid id)
        {
            var cargoPrice = await _repository.GetByIdAsync(id);

            if(cargoPrice is null)
            {
                throw new ValidationException("Cargo Price Not Found");
            }

            return cargoPrice.Adapt<UpdateCargoPriceDto>();
        }

        public async Task UpdateAsync(UpdateCargoPriceDto updateCargoPriceDto)
        {
            var cargoPrice = updateCargoPriceDto.Adapt<CargoPrice>();
            await _repository.UpdateAsync(cargoPrice);
        }
    }
}
