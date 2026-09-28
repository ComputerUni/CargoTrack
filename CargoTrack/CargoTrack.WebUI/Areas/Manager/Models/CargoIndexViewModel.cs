using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.ManagerCargoDtos;

namespace CargoTrack.WebUI.Areas.Manager.Models
{
    public class CargoIndexViewModel
    {
        public List<ResultCargoDto> Cargos { get; set; }
        public ManagerCargoIndexDto Summary { get; set; }
        public IncomingCargoSummaryDto Incoming { get; set; }
        public OutgoingCargoSummaryDto Outgoing { get; set; }
    }
}
