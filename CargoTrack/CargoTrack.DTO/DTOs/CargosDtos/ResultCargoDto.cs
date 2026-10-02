using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.CargosDtos
{
    public class ResultCargoDto
    {
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public double Weight { get; set; }
        public double Desi { get; set; }
        public decimal Price { get; set; }
        public int FailedAttemptCount { get; set; }
        public CargoType CargoType { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid OriginBranchId { get; set; }
        public string OriginBranchCity { get; set; }
        public string DestinationBranchCity { get; set; }
        public Guid DestinationBranchId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string OriginBranchName { get; set; }
        public string DestinationBranchName { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid? DeliveryAddressId { get; set; }
        public string DeliveryAddressDetail { get; set; }
        public string ReceiverPhone { get; set; }

        public Guid? AssignedEmployeeId { get; set; }
        public string AssignedCourierName { get; set; }
        public string AssignedCourierPhone { get; set; }

        public ExceptionReason? LastExceptionReason { get; set; }
        public string LastExceptionDescription { get; set; }
        public DateTime? LastExceptionDate { get; set; }



        public List<ResultCargoMovementDto> CargoMovements { get; set; } = new();
    }
}
