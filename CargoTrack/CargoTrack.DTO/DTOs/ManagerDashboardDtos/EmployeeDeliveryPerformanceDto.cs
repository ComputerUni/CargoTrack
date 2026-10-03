using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ManagerDashboardDtos
{
    public class EmployeeDeliveryPerformanceDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Initials { get; set; }
        public int TotalAssigned { get; set; }
        public int Delivered { get; set; }
        public int Remaining { get; set; }
        public int SuccessRate { get; set; }
    }
}
