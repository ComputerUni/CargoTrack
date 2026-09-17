using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.DeliveryExceptions
{
    public interface IDeliveryExceptionService
    {
        Task RecordDeliveryExceptionAsync(Guid cargoId, Guid employeeId, ExceptionReason exceptionReason, int attemptNumber, string description);
    }
}
