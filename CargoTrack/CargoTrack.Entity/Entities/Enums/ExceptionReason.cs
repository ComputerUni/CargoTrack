using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum ExceptionReason
    {
        [Display(Name = "Alıcı Bulunamadı")]
        RecipientNotFound = 1,

        [Display(Name = "Adres Hatalı")]
        WrongAddress = 2,

        [Display(Name = "Hasarlı Kargo")]
        DamagedCargo = 3,

        [Display(Name = "Kargo Reddedildi")]
        Rejected = 4,

        [Display(Name = "Diğer")]
        Other = 5

    }
}
