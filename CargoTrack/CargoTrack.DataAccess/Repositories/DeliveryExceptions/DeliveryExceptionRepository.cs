using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.DeliveryExceptions
{
    public class DeliveryExceptionRepository : GenericRepository<DeliveryException>, IDeliveryExceptionRepository
    {
        public DeliveryExceptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
