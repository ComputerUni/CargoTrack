using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.DeliveryDtos
{
    public class VerifyDeliveryDto
    {
        public Guid CargoId { get; set; }
        public string RecipientName { get; set; }
        public string DeliveryCode { get; set; }
        public Guid EmployeeId { get; set; }
        public string Note { get; set; }
    }
}
