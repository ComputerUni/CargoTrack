using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CargosDtos
{
    public class CargoStatusUpdateDto
    {
        public Guid Id { get; set; }
        public CargoStatus NewStatus { get; set; }
        public string Description { get; set; }
        public ExceptionReason? ExceptionReason { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? TransferCenterId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
