using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.DTO.DTOs.ManagerDashboardDtos;
using CargoTrack.Entity.Entities.Enums;
using Mapster;

namespace CargoTrack.Business.Services.ManagerDashboard
{
    public class ManagerDashboardService(IBranchRepository _branchRepository, ICargoRepository _cargoRepository, ICargoMovementRepository _cargoMovementRepository) : IManagerDashboardService
    {
        public async Task<List<EmployeeDeliveryPerformanceDto>> GetEmployeeDeliveryPerformance(Guid branchId)
        {
            var today = DateTime.Today;

            var cargos = await _cargoRepository.GetAllWithMovementsForDashboardAsync(branchId);

            var deliveryCargos = cargos.Where(x => x.AssignedEmployeeId.HasValue && x.DestinationBranchId == branchId && (x.CargoStatus == CargoStatus.OutForDelivery || x.CargoStatus == CargoStatus.Delivered || x.CargoStatus == CargoStatus.DeliveryFailed) &&
                                                x.CargoMovements.Any(m => m.NewStatus == CargoStatus.OutForDelivery && m.MovementDate.Date == today)).ToList();

            return deliveryCargos.GroupBy(x => x.AssignedEmployee)
                                 .Select(g => new EmployeeDeliveryPerformanceDto
                                 {
                                     Id = g.Key.Id,
                                     FullName = g.Key.FirstName + " " + g.Key.LastName,
                                     Initials = g.Key.FirstName.Substring(0, 1) + g.Key.LastName.Substring(0, 1),
                                     TotalAssigned = g.Count(),
                                     Delivered = g.Count(x => x.CargoStatus == CargoStatus.Delivered),
                                     Remaining = g.Count(x => x.CargoStatus == CargoStatus.OutForDelivery),
                                     SuccessRate = g.Count() > 0 ? (int)Math.Round((double)g.Count(x => x.CargoStatus == CargoStatus.Delivered) / g.Count() * 100) : 0
                                 }).ToList();
        }

        public async Task<List<ResultCargoMovementDto>> GetFailedCargoMovementsAsync(Guid branchId)
        {
            var allCargos = await _cargoMovementRepository.GetFailedByBranchIdAsync(branchId);
            return allCargos.Adapt<List<ResultCargoMovementDto>>();

        }

        public async Task<ManagerDashboardKpiDto> GetKpiAsync(Guid branchId)
        {
            var today = DateTime.Today;

            var allCargos = await _cargoRepository.GetAllWithMovementsForDashboardAsync(branchId);

            return new ManagerDashboardKpiDto
            {
                TodayIncoming = allCargos.Count(x => x.CargoMovements.Any(m => m.BranchId == branchId && m.NewStatus == CargoStatus.ArrivedAtDeliveryBranch && m.MovementDate.Date == today)),
                TodayOutgoing = allCargos.Count(x => x.CargoMovements.Any(m => m.BranchId == branchId && m.NewStatus == CargoStatus.InTransferCenter && m.MovementDate.Date == today)),
                OutForDelivery = allCargos.Count(x => x.CargoStatus == CargoStatus.OutForDelivery && x.DestinationBranchId == branchId),
                DeliveredToday = allCargos.Count(x => x.CargoStatus == CargoStatus.Delivered && x.CargoMovements.Any(m => m.NewStatus == CargoStatus.Delivered && m.MovementDate.Date == today)),
                Delayed = allCargos.Count(x => x.EstimatedArrivalDate < DateTime.Now && x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender && (x.DestinationBranchId == branchId || x.OriginBranchId == branchId)),
                Problematic = allCargos.Count(x => (x.CargoStatus == CargoStatus.DeliveryFailed || x.CargoStatus == CargoStatus.ReturnInProcess) && x.DestinationBranchId == branchId)
            };
        }

        public async Task<List<ResultCargoMovementDto>> GetResultCargoMovementsAsync(Guid branchId)
        {
            var movements = await _cargoMovementRepository.GetRecentByBranchIdAsync(branchId);
            return movements.Adapt<List<ResultCargoMovementDto>>();
        }
    }
}
