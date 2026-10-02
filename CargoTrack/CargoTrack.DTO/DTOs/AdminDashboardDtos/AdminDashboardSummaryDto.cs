using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardSummaryDto
    {
        public int TotalCargo { get; set; }
        public int ActiveCargo { get; set; }
        public int DelayedCargo { get; set; }
        public double DeliveryRate { get; set; }
    }
}
