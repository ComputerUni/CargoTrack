using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardTopBranchDto
    {
        public int Rank { get; set; }
        public string BranchName { get; set; }
        public int CargoCount { get; set; }
        public double Percentage { get; set; }
    }
}
