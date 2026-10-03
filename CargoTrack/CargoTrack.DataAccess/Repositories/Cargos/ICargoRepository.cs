using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.Cargos
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        Task<Cargo> GetByTrackCodeAsync(string trackCode);
        Task<List<Cargo>> GetAllWithDetailsAsync();
        Task<List<Cargo>> GetByBranchIdAsync(Guid branchId);
        Task<Cargo> GetByIdWithDetailsAsync(Guid id);
        Task<List<Cargo>> GetOutDeliveryByBranchIdAsync(Guid branchId);
        Task<List<Cargo>> GetDeliveryFailedOrReturnInProcessByBranchIdAsync(Guid branchId);
        Task<List<Cargo>> GetIncomingCargoAsync(Guid branchId);
        Task<List<Cargo>> GetOutgoingCargoAsync(Guid branchId);
        Task<Cargo> GetByIdWithMovementAsync(Guid id);
        Task<List<Cargo>> GetAllWithMovementsForDashboardAsync(Guid branchId);

             
    }
}
