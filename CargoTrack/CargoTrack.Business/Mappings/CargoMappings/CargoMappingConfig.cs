using CargoTrack.DTO.DTOs.CargoMovementDtos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using Mapster;

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
                .Map(dest => dest.DestinationBranchName, src => src.DestinationBranch.Name)
                .Map(dest => dest.DeliveryCode, src => src.DeliveryCode)
                .Map(dest => dest.DeliveryAddressDetail, src => src.DeliveryAddress != null ?
                 $"{src.DeliveryAddress.FullAddress}, {src.DeliveryAddress.District} / {src.DeliveryAddress.City}" : string.Empty)
                .Map(dest => dest.ReceiverPhone, src => src.Receiver != null ? src.Receiver.PhoneNumber : string.Empty)
                .Map(dest => dest.AssignedCourierName, src => src.AssignedEmployee != null
                ? $"{src.AssignedEmployee.FirstName} {src.AssignedEmployee.LastName}" : "Atama Bekliyor")
                .Map(dest => dest.AssignedCourierPhone, src => src.AssignedEmployee != null ? src.AssignedEmployee.Phone : string.Empty);

            TypeAdapterConfig<CargoMovement, ResultCargoMovementDto>.NewConfig()
                .Map(dest => dest.BranchName, src => src.Branch.Name)
                .Map(dest => dest.EmployeeName, src => src.Employee.FirstName + " " + src.Employee.LastName);
        }
    }
}
