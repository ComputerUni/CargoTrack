using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.UserCargos
{
    public interface IUserCargoRepository : IRepository<Cargo>
    {
        Task<List<Cargo>> GetByUserIdAsync(Guid userId);
        Task<List<Cargo>> GetSentByUserIdAsync(Guid userId);
        Task<List<Cargo>> GetReceivedByUserIdAsync(Guid userId);
        Task<List<Cargo>> GetDeliveredByUserIdAsync(Guid userId);
        Task<Cargo> GetByIdAsync(Guid userId, Guid cargoId);
        Task<Cargo> GetCargoDetailByCode(Guid userId, string trackCode);
    }
}
