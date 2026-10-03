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
    }
}
