using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class PerformanceReportDto
    {
        public ReportKpiDto KpiSummary { get; set; }
        public List<BranchPerformanceDto> BranchPerformances { get; set; }
        public List<DeliveryExceptionStatusDto> ExceptionStats { get; set; }
        public List<CourierPerformanceDto> TopCouriers { get; set; }
    }
}
