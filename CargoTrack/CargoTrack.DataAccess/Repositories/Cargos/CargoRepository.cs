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
                .Include(x => x.DeliveryAddress)
                .Include(x => x.AssignedEmployee)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetByBranchIdAsync(Guid branchId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.CargoMovements)
                .Where(x => x.OriginBranchId == branchId || x.DestinationBranchId == branchId)
                .ToListAsync();
        }

        //Kargo hareketleri için
        public async Task<Cargo> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Cargos
                .Include(x => x.DeliveryAddress)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.DeliveryExceptions)
                    .ThenInclude(e => e.Employee)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.Employee)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cargo> GetByTrackCodeAsync(string trackCode)
        {
            return await _context.Cargos
                .Include(x => x.DeliveryAddress)
                .Include(x => x.CargoMovements)
                .FirstOrDefaultAsync(x => x.TrackCode == trackCode);
        }

        public async Task<List<Cargo>> GetDeliveryFailedOrReturnInProcessByBranchIdAsync(Guid branchId)
        {
            return await _context.Cargos
              .Include(x => x.DeliveryAddress)
              .Include(x => x.AssignedEmployee)
              .Include(x => x.DeliveryExceptions)
                .ThenInclude(e => e.Employee)
              .Include(x => x.CargoMovements)
                .ThenInclude(x => x.Branch)
              .Include(x => x.CargoMovements)
                .ThenInclude(x => x.Employee)
              .Include(x => x.Sender)
              .Include(x => x.Receiver)
              .Include(x => x.OriginBranch)
              .Include(x => x.DestinationBranch)
              .Where(x => x.DestinationBranchId == branchId && x.CargoStatus == CargoStatus.DeliveryFailed || x.CargoStatus == CargoStatus.ReturnInProcess)
              .OrderByDescending(x => x.ShipmentDate)
              .ToListAsync();
        }

        public async Task<List<Cargo>> GetOutDeliveryByBranchIdAsync(Guid branchId)
        {
            return await _context.Cargos
              .Include(x => x.DeliveryAddress)
              .Include(x => x.AssignedEmployee)
              .Include(x => x.CargoMovements)
                .ThenInclude(x => x.Branch)
              .Include(x => x.CargoMovements)
                .ThenInclude(x => x.Employee)
              .Include(x => x.Sender)
              .Include(x => x.Receiver)
              .Include(x => x.OriginBranch)
              .Include(x => x.DestinationBranch)
              .Where(x => x.DestinationBranchId == branchId && x.CargoStatus == CargoStatus.OutForDelivery)
              .OrderByDescending(x => x.ShipmentDate)
              .ToListAsync();
        }
    }
}
