using CargoTrack.Business.Extensions;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.ReportDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.PerformanceReports
{
    public class PerformanceReportService(ICargoRepository _cargoRepository, IBranchRepository _branchRepository, IDeliveryExceptionRepository _deliveryExceptionRepository, IEmployeeRepository _employeeRepository) : IPerformanceReportService
    {
        public async Task<List<BranchPerformanceDto>> GetBranchPerformancesAsync()
        {
            var branches = await _branchRepository.GetAllAsync();
            var cargos = await _cargoRepository.GetAllAsync();

            var result = new List<BranchPerformanceDto>();

            foreach (var branch in branches)
            {
                var branchOutgoingCargos = cargos.Where(x => x.OriginBranchId == branch.Id).ToList();
                var totalOutgoing = branchOutgoingCargos.Count;

                if (totalOutgoing == 0) continue;

                var deliveredCount = branchOutgoingCargos.Count(x => x.CargoStatus == CargoStatus.Delivered);

                var successRate = totalOutgoing > 0 ? Math.Round((double)deliveredCount / totalOutgoing * 100, 1) : 0;

                var delayedCount = branchOutgoingCargos.Count(x => x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender && x.EstimatedArrivalDate < DateTime.Now);

                var returnCount = branchOutgoingCargos.Count(x => x.CargoStatus == CargoStatus.ReturnedToSender || x.CargoStatus == CargoStatus.ReturnInProcess);

                var returnRate = totalOutgoing > 0 ? Math.Round((double)returnCount / totalOutgoing * 100, 1) : 0;

                result.Add(new BranchPerformanceDto
                {
                    BranchId = branch.Id,
                    BranchName = branch.Name,
                    TotalOutgoing = totalOutgoing,
                    DeliveredCount = deliveredCount,
                    SuccessRate = successRate,
                    DelayedCount = delayedCount,
                    ReturnRate = returnRate
                });
            }

            return result.OrderByDescending(x => x.TotalOutgoing).ToList();

        }

        public async Task<List<CourierPerformanceDto>> GetCourierPerformancesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var branches = await _branchRepository.GetAllAsync();
            var cargos = await _cargoRepository.GetAllAsync();
            var deliveryExceptions = await _deliveryExceptionRepository.GetAllAsync();

            var result = new List<CourierPerformanceDto>();

            foreach (var employee in employees)
            {
                var employeeCargos = cargos.Where(x => x.AssignedEmployeeId == employee.Id).ToList();
                var totalEmployeeCargos = employeeCargos.Count;

                if (totalEmployeeCargos == 0) continue;

                var employeeDeliveredCargos = employeeCargos.Count(x => x.CargoStatus == CargoStatus.Delivered);
                var failedCount = deliveryExceptions.Count(x => x.EmployeeId == employee.Id);
                var totalAttempts = employeeDeliveredCargos + failedCount;

                var successRate = totalEmployeeCargos > 0 ? Math.Round((double)employeeDeliveredCargos / totalAttempts * 100, 1) : 0;

                var branchName = branches.FirstOrDefault(b => b.Id == employee.BranchId)?.Name;

                result.Add(new CourierPerformanceDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    BranchName = branchName,
                    DeliveredCount = employeeDeliveredCargos,
                    FailedCount = failedCount,
                    SuccessRate = successRate
                });
            }

            return result.OrderByDescending(x => x.SuccessRate).ThenByDescending(x => x.DeliveredCount).Take(5).ToList();
        }

        public async Task<List<DeliveryExceptionStatusDto>> GetDeliveryExceptionStatusesAsync()
        {
            var deliveryExceptions = await _deliveryExceptionRepository.GetAllAsync();
            var totalExceptions = deliveryExceptions.Count;

            var result = new List<DeliveryExceptionStatusDto>();

            if (totalExceptions == 0) return result;

            foreach (ExceptionReason reason in Enum.GetValues(typeof(ExceptionReason)))
            {
                var count = deliveryExceptions.Count(x => x.ExceptionReason == reason);
                if (count == 0) continue;

                var percentage = Math.Round((double)count / totalExceptions * 100, 1);

                result.Add(new DeliveryExceptionStatusDto
                {
                    ReasonText = reason.GetDisplayName(),
                    Count = count,
                    Percentage = percentage
                });
            }

            return result.OrderByDescending(x => x.Count).ToList();

        }

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
