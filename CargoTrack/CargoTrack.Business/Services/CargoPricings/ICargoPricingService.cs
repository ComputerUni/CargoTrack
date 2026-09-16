using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.CargoPricings
{
    public interface ICargoPricingService
    {
        Task<decimal> CalculatePriceAsync(double weight, double desi, CargoType cargoType, bool isIntercity);
        Task<DateTime> CalculateEstimatedDeliveryDateAsync(Guid originalBranchId, Guid destinationBranchId, CargoType cargoType);
    }
}
