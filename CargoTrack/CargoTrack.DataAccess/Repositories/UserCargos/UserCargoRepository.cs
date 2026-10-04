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

namespace CargoTrack.DataAccess.Repositories.UserCargos
{
    public class UserCargoRepository : GenericRepository<Cargo>, IUserCargoRepository
    {
        public UserCargoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Cargo>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
               .Include(x => x.OriginBranch)
               .Include(x => x.DestinationBranch)
               .Include(x => x.AssignedEmployee)
               .Include(x => x.Sender)
               .Include(x => x.Receiver)
               .Where(x => x.ReceiverId == userId)
               .OrderByDescending(x => x.CreatedDate)
               .ToListAsync();
        }

        public async Task<Cargo> GetByIdAsync(Guid userId, Guid cargoId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.TransferCenter)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryAddress)
                .FirstOrDefaultAsync(x => x.Id == cargoId && (x.SenderId == userId || x.ReceiverId == userId));
        }

        public async Task<List<Cargo>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x =>
                    (x.SenderId == userId || x.ReceiverId == userId) &&
                    x.CargoStatus != CargoStatus.Delivered &&
                    x.CargoStatus != CargoStatus.ReturnedToSender)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<Cargo> GetCargoDetailByCode(Guid userId, string trackCode)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.CargoMovements)
                    .ThenInclude(x => x.TransferCenter)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.DeliveryAddress)
                .FirstOrDefaultAsync(x => x.TrackCode == trackCode && (x.SenderId == userId || x.ReceiverId == userId));
        }

        public async Task<List<Cargo>> GetDeliveredByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.DeliveryExceptions)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Where(x => (x.ReceiverId == userId || x.SenderId == userId) &&
                            (x.CargoStatus == CargoStatus.Delivered || x.CargoStatus == CargoStatus.ReturnedToSender))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetFilteredDeliveredByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
               .Include(x => x.OriginBranch)
               .Include(x => x.DestinationBranch)
               .Include(x => x.AssignedEmployee)
               .Include(x => x.DeliveryExceptions)
               .Include(x => x.Receiver)
               .Include(x => x.Sender)
               .Where(x => (x.ReceiverId == userId || x.SenderId == userId) &&
                           (x.CargoStatus == CargoStatus.Delivered || x.CargoStatus == CargoStatus.ReturnedToSender))
               .OrderByDescending(x => x.CreatedDate)
               .ToListAsync();
        }

        public async Task<List<Cargo>> GetFilteredReceivedByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.Sender)
                .Include(x => x.DeliveryExceptions)
                .Where(x => x.ReceiverId == userId &&
                            x.CargoStatus != CargoStatus.Delivered &&
                            x.CargoStatus != CargoStatus.ReturnedToSender)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetFilteredSentByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetReceivedByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.Sender)
                .Include(x => x.DeliveryExceptions)
                .Where(x => x.ReceiverId == userId &&
                            x.CargoStatus != CargoStatus.Delivered &&
                            x.CargoStatus != CargoStatus.ReturnedToSender)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetSentByUserIdAsync(Guid userId)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch)
                .Include(x => x.DestinationBranch)
                .Include(x => x.AssignedEmployee)
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
