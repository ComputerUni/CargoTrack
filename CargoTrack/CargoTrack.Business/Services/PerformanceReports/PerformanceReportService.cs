using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.ReportDtos;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.PerformanceReports
{
    public class PerformanceReportService(ICargoRepository _cargoRepository) : IPerformanceReportService
    {
        public async Task<ReportKpiDto> GetKpiSummaryAsync()
        {
            var cargos = await _cargoRepository.GetAllWithDetailsAsync();
            var totalCargo = cargos.Count;
            var deliveredCargos = cargos.Where(x => x.CargoStatus == CargoStatus.Delivered).ToList();
            var deliveredCount = deliveredCargos.Count;

            var onTimeDeliveryCount = deliveredCargos.Count(x => (x.Delivery != null && x.Delivery.DeliveryDate <= x.EstimatedArrivalDate) ||
                    (x.Delivery != null && x.UpdatedDate <= x.EstimatedArrivalDate));

            var onTimeRate = deliveredCount > 0 ? Math.Round((double)onTimeDeliveryCount / deliveredCount * 100, 1) : 0;

            var totalDeliveryHours = deliveredCargos.Select(x => ((x.Delivery?.DeliveryDate ?? x.UpdatedDate ?? DateTime.Now) - x.ShipmentDate).TotalHours)
                                                    .Where(hours => hours > 0).ToList();

            var avgDeliveryHours = totalDeliveryHours.Any() ? Math.Round(totalDeliveryHours.Average(), 1) : 0;

            var returnCount = cargos.Count(x => x.CargoStatus == CargoStatus.ReturnedToSender || x.CargoStatus == CargoStatus.ReturnInProcess);

            var returnRate = Math.Round((double)returnCount / totalCargo * 100, 1);

            var delayedCount = cargos.Count(x => x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender && x.EstimatedArrivalDate < DateTime.Now);

            var delayedRate = Math.Round((double)delayedCount / totalCargo * 100, 1);

            return new ReportKpiDto
            {
                OnTimeDeliveryRate = onTimeRate,
                AverageDeliveryTimeInHours = avgDeliveryHours,
                TotalCargoCount = totalCargo,
                TotalReturnCount = returnCount,
                ReturnRate = returnRate,
                DelayedDeliveryRate = delayedRate
            };
        }
    }
}
