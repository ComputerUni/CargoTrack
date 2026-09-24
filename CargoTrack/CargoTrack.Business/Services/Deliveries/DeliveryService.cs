using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Deliveries
{
    public class DeliveryService(IDeliveryRepository _repository, ICargoRepository _cargoRepository, ICargoMovementRepository _movementRepository) : IDeliveryService
    {
        public async Task<string> GenerateDeliveryCodeAsync(Guid cargoId)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);
            var random = new Random().Next(100000, 999999);
            var code = random.ToString();
            cargo.DeliveryCode = code;
            await _cargoRepository.UpdateAsync(cargo);
            return code;
           
        }

        public async Task VerifyAndCompleteDeliveryAsync(Guid cargoId, string deliveryCode, string receiverName, Guid employeeId, string note)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);
            if(cargo.CargoStatus != CargoStatus.OutForDelivery)
            {
                throw new Exception("Kargo Dağıtıma Çıkmamıştır.");
            }
            if(cargo.DeliveryCode != deliveryCode)
            {
                throw new Exception("Doğrulama Kodu Yanlıştır.");
            }
            var delivery = new Delivery
            {
                CargoId = cargoId,
                DeliveryDate = DateTime.Now,
                RecipientName = receiverName,
                DeliveryCode = deliveryCode,
                EmployeeId = employeeId,
                Note = note
            };
            await _repository.CreateAsync(delivery);
            var movement = new CargoMovement
            {
                CargoId = cargoId,
                PreviousStatus = cargo.CargoStatus,
                NewStatus = CargoStatus.Delivered,
                MovementDate = DateTime.Now,
                BranchId = cargo.DestinationBranchId,
                TransferCenterId = null,
                EmployeeId = employeeId,
                Description = "Kargo teslim edildi"
            };

            await _movementRepository.CreateAsync(movement);
            cargo.CargoStatus = CargoStatus.Delivered;
            await _cargoRepository.UpdateAsync(cargo);
        }
    }
}
