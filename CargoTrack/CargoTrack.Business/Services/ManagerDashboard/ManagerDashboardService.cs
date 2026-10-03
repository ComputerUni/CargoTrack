using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.ManagerDashboardDtos;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.ManagerDashboard
{
    public class ManagerDashboardService(IBranchRepository _branchRepository, ICargoRepository _cargoRepository) : IManagerDashboardService
    {
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
    }
}
