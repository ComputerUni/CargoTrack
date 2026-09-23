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
        public Guid DestinationBranchId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string OriginBranchName { get; set; }
        public string DestinationBranchName { get; set; }
        public List<ResultCargoMovementDto> CargoMovements { get; set; } = new();
    }
}
