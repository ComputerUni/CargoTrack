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
    }
}
