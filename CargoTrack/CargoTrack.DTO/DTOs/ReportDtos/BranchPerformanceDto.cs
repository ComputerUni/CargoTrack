using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class BranchPerformanceDto
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public int TotalOutgoing { get; set; }
        public int DeliveredCount { get; set; }
        public double SuccessRate { get; set; }
        public int DelayedCount { get; set; }
        public double ReturnRate { get; set; }
        public string PerformanceScore => SuccessRate switch
        {
            >= 95 => "A+",
            >= 90 => "A",
            >= 80 => "B+",
            >= 70 => "B",
            >= 60 => "C",
            _ => "D"
        };
    }
}
