using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.AdminDashboardDtos
{
    public class AdminDashboardRecentCargosDto
    {
        public string TrackingNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Route { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
