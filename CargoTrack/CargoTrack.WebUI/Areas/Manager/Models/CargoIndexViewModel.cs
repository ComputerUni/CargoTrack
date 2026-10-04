using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.ManagerCargoDtos;
using X.PagedList;

namespace CargoTrack.WebUI.Areas.Manager.Models
{
    public class CargoIndexViewModel
    {
        public IPagedList<ResultCargoDto> Cargos { get; set; }
        public ManagerCargoIndexDto Summary { get; set; }
        public IncomingCargoSummaryDto Incoming { get; set; }
        public OutgoingCargoSummaryDto Outgoing { get; set; }
    }
}
