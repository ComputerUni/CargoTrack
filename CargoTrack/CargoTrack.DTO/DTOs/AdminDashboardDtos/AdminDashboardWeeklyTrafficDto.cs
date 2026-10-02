using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardWeeklyTrafficDto
    {
        public string Day { get; set; }
        public int IncomingCargo { get; set; }
        public int OutgoingCargo { get; set; }
        public double IncomingPercent { get; set; }
        public double OutgoingPercent { get; set; }
    }
}
