using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Deliveries
{
    public interface IDeliveryService
    {
        Task<string> GenerateDeliveryCodeAsync(Guid cargoId);
        Task VerifyAndCompleteDeliveryAsync(Guid cargoId, string deliveryCode, string receiverName, Guid employeeId, string note);
    }
}
