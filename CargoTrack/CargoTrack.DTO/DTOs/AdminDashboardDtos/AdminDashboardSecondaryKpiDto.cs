using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardSecondaryKpiDto
    {
        public int TotalBranch { get; set; }
        public int TransferCenter { get; set; }
        public int TotalEmployee { get; set; }
        public double DeliveredRate { get; set; }
    }
}
