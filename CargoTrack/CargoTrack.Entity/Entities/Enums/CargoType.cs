using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoType
    {
        [Display(Name = "Standart")]
        Standart = 1,
        [Display(Name = "Acil")]
        Urgent = 2,
        [Display(Name = "Kırılgan")]
        Fragile = 3,
        [Display(Name = "Ağır")]
        Heavy = 4
    }
}
