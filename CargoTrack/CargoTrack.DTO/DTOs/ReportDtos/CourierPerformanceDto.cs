using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class CourierPerformanceDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string BranchName { get; set; }
        public int DeliveredCount { get; set; }
        public int FailedCount { get; set; }
        public double SuccessRate { get; set; }
    }
}
