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

        public async Task<List<CargoMovement>> GetFailedByBranchIdAsync(Guid branchId)
        {
            return await _context.CargoMovements.Include(x => x.Branch)
                .Include(x => x.TransferCenter)
                .Include(x => x.Employee)
                .Include(x => x.Cargo)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.Cargo)
                     .ThenInclude(x => x.Receiver)
                 .Where(x => x.BranchId == branchId &&
                 (x.Cargo.CargoStatus == CargoStatus.DeliveryFailed || x.Cargo.CargoStatus == CargoStatus.ReturnInProcess || x.Cargo.CargoStatus == CargoStatus.ReturnedToSender))
                 .OrderByDescending(x => x.MovementDate).Take(5).ToListAsync();
        }

        public async Task<List<CargoMovement>> GetRecentByBranchIdAsync(Guid branchId)
        {
            return await _context.CargoMovements.Include(x => x.Branch)
                .Include(x => x.TransferCenter)
                .Include(x => x.Employee)
                .Include(x => x.Cargo)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.Cargo)
                     .ThenInclude(x => x.Receiver)
                 .Where(x => x.BranchId == branchId)
                 .OrderByDescending(x => x.MovementDate).Take(10).ToListAsync();
        }
    }
}
