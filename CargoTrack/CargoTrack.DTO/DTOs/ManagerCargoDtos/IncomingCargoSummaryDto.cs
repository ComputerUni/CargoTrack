using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ManagerCargoDtos
{
    public class IncomingCargoSummaryDto
    {
        public int TotalIncoming { get; set; }
        public int WaitingAtBranch { get; set; }
        public int DeliveredToday { get; set; }
        public int Delayed { get; set; }
    }
}
