using CargoTrack.DTO.DTOs.CargoMovementDtos;
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
                .Map(dest => dest.ReceiverName, src => src.Receiver.FirstName + " " + src.Receiver.LastName)
                .Map(dest => dest.OriginBranchName, src => src.OriginBranch.Name)
                .Map(dest => dest.DestinationBranchName, src => src.DestinationBranch.Name);

            TypeAdapterConfig<CargoMovement, ResultCargoMovementDto>.NewConfig()
                .Map(dest => dest.BranchName, src => src.Branch.Name)
                .Map(dest => dest.EmployeeName, src => src.Employee.FirstName + " " + src.Employee.LastName);
            
        }
    }
}
