using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class ReportKpiDto
    {
        public double OnTimeDeliveryRate { get; set; }
        public double AverageDeliveryTimeInHours { get; set; }
        public double ReturnRate { get; set; }
        public int TotalReturnCount { get; set; }
        public int TotalCargoCount { get; set; }
        public double DelayedDeliveryRate { get; set; }
    }
}
