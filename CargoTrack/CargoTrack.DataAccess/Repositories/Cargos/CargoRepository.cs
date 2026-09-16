using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.Cargos
{
    public class CargoRepository : GenericRepository<Cargo>, ICargoRepository
    {
        public CargoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cargo> GetByTrackCodeAsync(string trackCode)
        {
            return await _context.Cargos.FirstOrDefaultAsync(x => x.TrackCode == trackCode);
        }
    }
}
