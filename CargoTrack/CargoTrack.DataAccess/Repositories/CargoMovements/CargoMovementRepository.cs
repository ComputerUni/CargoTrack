using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.CargoMovements
{
    public class CargoMovementRepository : GenericRepository<CargoMovement>, ICargoMovementRepository
    {
        public CargoMovementRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CargoMovement>> GetByCargoIdAsync(Guid cargoId)
        {
            return await _context.CargoMovements.Where(x => x.CargoId == cargoId).OrderBy(x => x.MovementDate).ToListAsync();
        }
    }
}
