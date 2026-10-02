using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardStatusDonutDto
    {
        public int TotalCargo { get; set; }
        public int Delivered { get; set; }
        public int Active { get; set; }
        public int InTransfer { get; set; }
        public int Delayed { get; set; }
        public int Other { get; set; }

        public double DeliveredPercent { get; set; }
        public double ActivePercent { get; set; }
        public double InTransferPercent { get; set; }
        public double DelayedPercent { get; set; }
        public double OtherPercent { get; set; }
    }
}
