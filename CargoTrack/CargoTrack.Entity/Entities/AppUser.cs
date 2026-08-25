using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        //Navigation Properties
        public IList<Cargo> SentCargos { get; set; }
        public IList<Cargo> ReceivedCargos { get; set; }
        public IList<Address> Addresses { get; set; }
    }
}
