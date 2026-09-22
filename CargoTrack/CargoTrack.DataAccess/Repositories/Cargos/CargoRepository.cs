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

        public async Task<List<Cargo>> GetAllWithDetailsAsync()
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetByBranchIdAsync(Guid branchId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.OriginBranch)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x => x.OriginBranchId == branchId || x.DestinationBranchId == branchId)
                .ToListAsync();
        }

        public async Task<Cargo> GetByTrackCodeAsync(string trackCode)
        {
            return await _context.Cargos.FirstOrDefaultAsync(x => x.TrackCode == trackCode);
        }
    }
}
