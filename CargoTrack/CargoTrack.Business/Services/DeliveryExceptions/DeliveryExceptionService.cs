using CargoTrack.Business.Services.Cargos;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.DeliveryExceptions
{
    public class DeliveryExceptionService(IDeliveryExceptionRepository _deliveryExceptionRepository, ICargoRepository _cargoRepository, ICargoMovementRepository _movementRepository) : IDeliveryExceptionService
    {
        public async Task RecordDeliveryExceptionAsync(Guid cargoId, Guid employeeId, ExceptionReason exceptionReason, int attemptNumber, string description)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);
            if (cargo.CargoStatus != CargoStatus.OutForDelivery)
            {
                throw new Exception("Kargo Dağıtıma Çıkmamış Durumunda Değildir.");
            }
            var deliveryException = new DeliveryException
            {
                CargoId = cargoId,
                EmployeeId = employeeId,
                ExceptionReason = exceptionReason,
                AttemptNumber = attemptNumber,
                Description = description
            };

            await _deliveryExceptionRepository.CreateAsync(deliveryException);

            cargo.FailedAttemptCount++;

            if(cargo.FailedAttemptCount >= 3)
            {
                var movement = new CargoMovement
                {
                    CargoId = cargoId,
                    EmployeeId = employeeId,
                    PreviousStatus = CargoStatus.OutForDelivery,
                    NewStatus = CargoStatus.ReturnInProcess,
                    MovementDate = DateTime.Now,
                    Description = description,
                    BranchId = cargo.DestinationBranchId,
                    TransferCenterId = null,
                };

                await _movementRepository.CreateAsync(movement);
                cargo.CargoStatus = CargoStatus.ReturnInProcess;
            }

            await _cargoRepository.UpdateAsync(cargo);
            
        }
    }
}
