using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoStatus
    {
        [Display(Name = "Oluşturuldu")]
        Received = 1,

        [Display(Name = "Gönderici Şubesinde")]
        InTransferCenter = 2,

        [Display(Name = "Transfer Merkezinde")]
        DispatchedFromTransferCenter = 3,

        [Display(Name = "Varış Şubesinde")]
        ArrivedAtDeliveryBranch = 4,

        [Display(Name = "Dağıtıma Çıktı")]
        OutForDelivery = 5,

        [Display(Name = "Teslim Edildi")]
        Delivered = 6,

        [Display(Name = "Teslim Edilemedi")]
        DeliveryFailed = 7,

        [Display(Name = "İade Sürecinde")]
        ReturnInProcess = 8,

        [Display(Name = "Göndericiye İade Edildi")]
        ReturnedToSender = 9
    }
}
