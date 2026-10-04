using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminBranchDtos
{
    public class AdminBranchSummaryDto
    {
        public int TotalBranches { get; set; }
        public int ActiveBranches { get; set; }
        public int TotalEmployees { get; set; }
        public double AvgEmployeePerBranch { get; set; }
        public int ActiveCargos { get; set; }
    }
}
