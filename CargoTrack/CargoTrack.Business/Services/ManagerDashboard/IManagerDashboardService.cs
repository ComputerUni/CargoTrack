using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.DTO.DTOs.ManagerDashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.ManagerDashboard
{
    public interface IManagerDashboardService
    {
        Task<ManagerDashboardKpiDto> GetKpiAsync(Guid branchId);
        Task<List<EmployeeDeliveryPerformanceDto>> GetEmployeeDeliveryPerformance(Guid branchId);
        Task<List<ResultCargoMovementDto>> GetResultCargoMovementsAsync(Guid branchId);
        Task<List<ResultCargoMovementDto>> GetFailedCargoMovementsAsync(Guid branchId);
    }
}
