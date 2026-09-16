using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.CargoPrices
{
    public class CargoPricesRepository : GenericRepository<CargoPrice>, ICargoPricesRepository
    {
        public CargoPricesRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CargoPrice> GetMatchingRuleAsync(double weight, CargoType cargoType, bool isIntercity)
        {
            return await _context.CargoPrices.FirstOrDefaultAsync(x => x.CargoType == cargoType && x.IsIntercity == isIntercity && x.MinWeight <= weight && x.MaxWeight >= weight);
        }
    }
}
