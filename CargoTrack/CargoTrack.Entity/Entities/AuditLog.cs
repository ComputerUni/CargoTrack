using CargoTrack.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class AuditLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public string ActionType { get; set; }
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string Description { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual AppUser User { get; set; }

    }
}
