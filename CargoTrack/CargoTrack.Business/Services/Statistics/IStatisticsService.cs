using CargoTrack.DTO.DTOs.AdminDashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<AdminDashboardSummaryDto> GetAdminDashboardSummaryAsync();
        Task<List<AdminDashboardWeeklyTrafficDto>> GetWeeklyTrafficAsync();
        Task<AdminDashboardStatusDonutDto> GetStatusDonutAsync();
        Task<List<AdminDashboardTopBranchDto>> GetTopBranchAsync();
        Task<AdminDashboardSecondaryKpiDto> GetAdminDashboardSecondaryKpiAsync();
        Task<List<AdminDashboardRecentCargosDto>> GetRecentCargosAsync();
    }
}
