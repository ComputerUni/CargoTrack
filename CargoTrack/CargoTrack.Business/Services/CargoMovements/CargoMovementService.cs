using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.CargoMovements
{
    internal class CargoMovementService(ICargoMovementRepository _repository, ICargoRepository _cargoRepository) : ICargoMovementService
    {
        public async Task CreateMovementAsync(Guid cargoId, CargoStatus newStatus, Guid? branchId, Guid? transferCenterId, Guid? employeeId, string description)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);

            var allowedTransitions = new Dictionary<CargoStatus, List<CargoStatus>>
            {
                { CargoStatus.Created, new List<CargoStatus> { CargoStatus.AtOriginBranch} },
                { CargoStatus.AtOriginBranch, new List<CargoStatus> { CargoStatus.InTransferCenter} },
                { CargoStatus.InTransferCenter, new List<CargoStatus> { CargoStatus.ArrivedAtDeliveryBranch} },
                { CargoStatus.ArrivedAtDeliveryBranch, new List<CargoStatus> { CargoStatus.OutForDelivery} },
                { CargoStatus.OutForDelivery, new List<CargoStatus> { CargoStatus.Delivered, CargoStatus.DeliveryFailed} },
                { CargoStatus.DeliveryFailed, new List<CargoStatus> { CargoStatus.OutForDelivery, CargoStatus.ReturnInProcess} },
                { CargoStatus.ReturnInProcess, new List<CargoStatus> { CargoStatus.ReturnedToSender} },
            };

            if (!allowedTransitions[cargo.CargoStatus].Contains(newStatus))
                throw new Exception("Bu durum geçişine izin verilmiyor.");

            var movement = new CargoMovement
            {
                CargoId = cargoId,
                PreviousStatus = cargo.CargoStatus,
                BranchId = branchId,
                TransferCenterId = transferCenterId,
                EmployeeId = employeeId,
                Description = description,
                NewStatus = newStatus,
                MovementDate = DateTime.Now
            };

            cargo.CargoStatus = newStatus;

            await _cargoRepository.UpdateAsync(cargo);
            await _repository.CreateAsync(movement);

          
        }

        public async Task<List<ResultCargoMovementDto>> GetByCargoIdAsync(Guid cargoId)
        {
            var cargo = await _repository.GetByCargoIdAsync(cargoId);
            return cargo.Adapt<List<ResultCargoMovementDto>>();
        }
    }
}
