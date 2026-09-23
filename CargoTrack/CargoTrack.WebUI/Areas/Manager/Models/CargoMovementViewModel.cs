using CargoTrack.DTO.DTOs.CargosDtos;

namespace CargoTrack.WebUI.Areas.Manager.Models
{
    public class CargoMovementViewModel
    {
        public ResultCargoDto Cargo { get; set; }
        public CargoStatusUpdateDto StatusUpdate{ get; set; }
    }
}
