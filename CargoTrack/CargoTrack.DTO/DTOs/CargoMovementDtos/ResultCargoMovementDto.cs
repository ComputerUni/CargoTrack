using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CargoMovementDtos
{
    public class ResultCargoMovementDto
    {
        public Guid CargoId { get; set; }
        public CargoStatus PreviousStatus { get; set; }
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string Description { get; set; }
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
        public Guid? TransferCenterId { get; set; }
        public string? TransferCenterName { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string TrackCode { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
