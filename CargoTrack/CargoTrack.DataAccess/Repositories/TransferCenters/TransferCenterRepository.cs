using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.TransferCenters
{
    public class TransferCenterRepository : GenericRepository<TransferCenter>, ITransferCenterRepository
    {
        public TransferCenterRepository(AppDbContext context) : base(context)
        {
        }
    }
}
