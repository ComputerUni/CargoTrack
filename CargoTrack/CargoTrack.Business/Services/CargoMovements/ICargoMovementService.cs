using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.CargoMovements
{
    public interface ICargoMovementService
    {
        Task CreateMovementAsync(Guid cargoId, CargoStatus newStatus, Guid? branchId, Guid? transferCenterId, Guid? employeeId, string description);
        Task<List<ResultCargoMovementDto>> GetByCargoIdAsync(Guid cargoId);
    }
}
