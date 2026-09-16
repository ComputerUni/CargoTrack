using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.DeliveryExceptionDtos
{
    public class CreateDeliveryExceptionDto
    {
        public Guid CargoId { get; set; }
        public Guid EmployeeId { get; set; }
        public ExceptionReason ExceptionReason { get; set; }
        public string Description { get; set; }
    }
}
