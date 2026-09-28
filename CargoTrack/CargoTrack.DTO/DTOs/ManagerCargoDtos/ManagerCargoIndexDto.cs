using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ManagerCargoDtos
{
    public class ManagerCargoIndexDto
    {
        public int TotalCargos { get; set; }
        public int OutOfDelivery { get; set; }
        public int ArrivedAtBranch { get; set; }
        public int AtOriginBranch { get; set; }
        public int DeliveryFailed { get; set; }
        public int Delivered { get; set; }
    }
}
