using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.ManagerCargoDtos
{
    public class OutgoingCargoSummaryDto
    {
        public int TotalOutgoing { get; set; }
        public int InTransferCenter { get; set; }
        public int ReturnedToSender { get; set; }
        public int OutOfDelivery { get; set; }
    }
}
