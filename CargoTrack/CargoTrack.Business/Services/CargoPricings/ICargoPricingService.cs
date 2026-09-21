using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.CargoPricings
{
    public interface ICargoPricingService
    {
        Task<List<ResultCargoPriceDto>> GetAllAsync();
        Task<UpdateCargoPriceDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateCargoPriceDto createCargoPriceDto);
        Task UpdateAsync(UpdateCargoPriceDto updateCargoPriceDto);
        Task DeleteAsync(Guid id);
        Task<decimal> CalculatePriceAsync(double weight, double desi, CargoType cargoType, bool isIntercity);
        Task<DateTime> CalculateEstimatedDeliveryDateAsync(Guid originalBranchId, Guid destinationBranchId, CargoType cargoType);
        Task<string> GenerateTrackCode();
    }
}
