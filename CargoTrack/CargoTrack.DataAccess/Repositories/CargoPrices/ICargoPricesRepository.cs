using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.CargoPrices
{
    public interface ICargoPricesRepository : IRepository<CargoPrice>
    {
        Task<CargoPrice> GetMatchingRuleAsync(double weight, CargoType cargoType, bool isIntercity);
    }
}
