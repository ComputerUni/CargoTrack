using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Mappings.CargoMappings
{
    public class CargoMappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Cargo, ResultCargoDto>.NewConfig()
                .Map(dest => dest.SenderName, src => src.Sender.FirstName + " " + src.Sender.LastName)
                .Map(dest => dest.ReceiverName, src => src.Receiver.FirstName + " " + src.Receiver.LastName);
        }
    }
}
