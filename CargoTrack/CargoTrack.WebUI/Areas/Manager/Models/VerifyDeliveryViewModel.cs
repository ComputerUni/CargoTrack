using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.DeliveryDtos;

namespace CargoTrack.WebUI.Areas.Manager.Models
{
    public class VerifyDeliveryViewModel
    {
        public ResultCargoDto Cargo { get; set; }
        public VerifyDeliveryDto DeliveryInput { get; set; }
    }
}
