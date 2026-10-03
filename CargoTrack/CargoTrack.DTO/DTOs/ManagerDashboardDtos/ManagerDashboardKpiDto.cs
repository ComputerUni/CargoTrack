using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ManagerDashboardDtos
{
    public class ManagerDashboardKpiDto
    {
        public int TodayIncoming { get; set; }
        public int TodayOutgoing { get; set; }
        public int OutForDelivery { get; set; }
        public int DeliveredToday { get; set; }
        public int Delayed { get; set; }
        public int Problematic { get; set; }
    }
}
