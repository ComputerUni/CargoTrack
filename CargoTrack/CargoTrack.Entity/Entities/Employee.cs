using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Guid BranchId { get; set; }


        public virtual Branch Branch { get; set; }
        public virtual IList<CargoMovement> CargoMovements { get; set; }
        public virtual IList<Delivery> Deliveries { get; set; }
        public virtual IList<DeliveryException> DeliveryExceptions { get; set; }
    }
}
