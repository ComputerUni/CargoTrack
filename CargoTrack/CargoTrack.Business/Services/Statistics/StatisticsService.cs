using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.DTO.DTOs.AdminDashboardDtos;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Statistics
{
    public class StatisticsService(ICargoRepository _cargoRepository, IBranchRepository _branchRepository, ITransferCenterRepository _transferCenterRepository, IEmployeeRepository _employeeRepository) : IStatisticsService
    {
        public async Task<AdminDashboardSecondaryKpiDto> GetAdminDashboardSecondaryKpiAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var branches = await _branchRepository.GetAllAsync();
            var transfer = await _transferCenterRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();
            var total = branches.Count;
            var transferTotal = transfer.Count;
            var employeesTotal = employees.Count;
            var cargosTotal = cargos.Count;
            var deliveredCargos = cargos.Count(x => x.CargoStatus == CargoStatus.ReturnedToSender);
            var deliveredCargosRate = cargosTotal > 0 ? Math.Round((double)deliveredCargos / cargosTotal * 100, 1) : 0;

            return new AdminDashboardSecondaryKpiDto
            {
                TotalBranch = total,
                TransferCenter = transferTotal,
                TotalEmployee = employeesTotal,
                DeliveredRate = deliveredCargosRate,
            };
        }

        public async Task<AdminDashboardSummaryDto> GetAdminDashboardSummaryAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var total = cargos.Count();
            var active = cargos.Count(x => x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender);
            var delayed = cargos.Count(x => x.EstimatedArrivalDate < DateTime.Now && x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender);
            var delivered = cargos.Count(x => x.CargoStatus == CargoStatus.Delivered);
            var deliveryRate = total > 0 ? Math.Round((double)delivered / total * 100, 1) : 0;
            return new AdminDashboardSummaryDto
            {
                TotalCargo = total,
                ActiveCargo = active,
                DelayedCargo = delayed,
                DeliveryRate = deliveryRate
            };
        }

        public async Task<List<AdminDashboardTopBranchDto>> GetTopBranchAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();

            var topBranches = cargos.Where(x => x.OriginBranch != null)
                                    .GroupBy(x => x.OriginBranch.Name)
                                    .Select(g => new
                                    {
                                        Name = g.Key,
                                        Count = g.Count()
                                    }).OrderByDescending(x => x.Count).Take(5).ToList();

            var maxCount = topBranches.First().Count;

            return topBranches.Select((b, index) => new AdminDashboardTopBranchDto
            {
                Rank = index + 1,
                BranchName = b.Name,
                CargoCount = b.Count,
                Percentage = maxCount > 0 ? Math.Round((double)b.Count / maxCount * 100, 1) : 0
            }).ToList();

        }

        public async Task<List<AdminDashboardRecentCargosDto>> GetRecentCargosAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var cargoMovements = cargos.OrderByDescending(x => x.CreatedDate).Take(5).Select(g => new AdminDashboardRecentCargosDto
            {
                TrackingNumber = g.TrackCode,
                SenderName = $"{g.Sender.FirstName} {g.Sender.LastName}",
                ReceiverName = $"{g.Receiver.FirstName} {g.Receiver.LastName}",
                Route = $"{g.OriginBranch.Name} → {g.DestinationBranch.Name}",
                CargoStatus = g.CargoStatus,
                CreatedDate = g.CreatedDate
            }).ToList();

            return cargoMovements;      
        }

        public async Task<AdminDashboardStatusDonutDto> GetStatusDonutAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var total = cargos.Count();
            var delivered = cargos.Count(x => x.CargoStatus == CargoStatus.Delivered);
            var active = cargos.Count(x => x.CargoStatus == CargoStatus.AtOriginBranch || x.CargoStatus == CargoStatus.OutForDelivery || x.CargoStatus == CargoStatus.ArrivedAtDeliveryBranch);
            var inTransfer = cargos.Count(x => x.CargoStatus == CargoStatus.InTransferCenter);
            var delayed = cargos.Count(x => x.EstimatedArrivalDate < DateTime.Today || x.CargoStatus == CargoStatus.DeliveryFailed);
            var other = cargos.Count(x => x.CargoStatus == CargoStatus.Created || x.CargoStatus == CargoStatus.ReturnInProcess || x.CargoStatus == CargoStatus.ReturnedToSender || x.CargoStatus == CargoStatus.DeliveryFailed);

            var deliveredPercent = total > 0 ? Math.Round((double)delivered / total * 100, 1) : 0;
            var activePercent = total > 0 ? Math.Round((double)active / total * 100, 1) : 0;
            var inTransferPercent = total > 0 ? Math.Round((double)inTransfer / total * 100, 1) : 0;
            var delayedPercent = total > 0 ? Math.Round((double)delayed / total * 100, 1) : 0;
            var otherPercent = total > 0 ? Math.Round((double)other / total * 100, 1) : 0;

            return new AdminDashboardStatusDonutDto
            {
                TotalCargo = total,
                Delivered = delivered,
                Active = active,
                InTransfer = inTransfer,
                Delayed = delayed,
                Other = other,
                DeliveredPercent = deliveredPercent,
                ActivePercent = activePercent,
                InTransferPercent = inTransferPercent,
                DelayedPercent = delayedPercent,
                OtherPercent = otherPercent,
            };
        }

        public async Task<List<AdminDashboardWeeklyTrafficDto>> GetWeeklyTrafficAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var today = DateTime.Today;
            var days = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();

            var result = days.Select(day => new AdminDashboardWeeklyTrafficDto
            {
                Day = day.DayOfWeek switch
                {
                    DayOfWeek.Monday => "Pzt",
                    DayOfWeek.Tuesday => "Sal",
                    DayOfWeek.Wednesday => "Çar",
                    DayOfWeek.Thursday => "Per",
                    DayOfWeek.Friday => "Cum",
                    DayOfWeek.Saturday => "Cmt",
                    DayOfWeek.Sunday => "Paz",
                    _ => ""
                },
                OutgoingCargo = cargos.Count(x => x.CreatedDate.Date == day.Date),
                IncomingCargo = cargos.Count(x => x.CreatedDate.Date == day.Date &&
                                            (x.CargoStatus == CargoStatus.Delivered || x.CargoStatus == CargoStatus.ArrivedAtDeliveryBranch))
            }).ToList();

            var max = result.Max(x => Math.Max(x.IncomingCargo, x.OutgoingCargo));
            max = max == 0 ? 1 : max;

            foreach (var item in result)
            {
                item.IncomingPercent = (int)(item.IncomingCargo * 100.0 / max);
                item.OutgoingPercent = (int)(item.OutgoingCargo * 100.0 / max);
            }

            return result;
        }
    }
}
