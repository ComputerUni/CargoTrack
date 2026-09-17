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
    public class DeliveryService(IDeliveryRepository _repository, ICargoRepository _cargoRepository) : IDeliveryService
    {
        public async Task<string> GenerateDeliveryCodeAsync(Guid cargoId)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);
            if(cargo.CargoStatus != CargoStatus.OutForDelivery)
            {
                throw new Exception("Kargo Dağıtıma Çıkmamıştır.");
            }
            var random = new Random().Next(100000, 999999);
            var code = random.ToString();
            cargo.Delivery.DeliveryCode = code;
            await _cargoRepository.UpdateAsync(cargo);
            return code;
           
        }
    }
}
