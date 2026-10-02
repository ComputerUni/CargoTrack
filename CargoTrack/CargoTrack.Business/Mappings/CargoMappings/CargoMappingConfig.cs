using CargoTrack.DTO.DTOs.AuditLogDtos;
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
                .Map(dest => dest.AssignedCourierPhone, src => src.AssignedEmployee != null ? src.AssignedEmployee.Phone : string.Empty)
                .Map(dest => dest.LastExceptionDescription, src => src.DeliveryExceptions != null && src.DeliveryExceptions.Any() ? src.DeliveryExceptions.OrderByDescending(e => e.CreatedDate).FirstOrDefault().Description : null)
                .Map(dest => dest.OriginBranchCity, src => src.OriginBranch.City.Name)
                .Map(dest => dest.DestinationBranchCity, src => src.DestinationBranch.City.Name);

            TypeAdapterConfig<CargoMovement, ResultCargoMovementDto>.NewConfig()
                .Map(dest => dest.BranchName, src => src.Branch.Name)
                .Map(dest => dest.EmployeeName, src => src.Employee.FirstName + " " + src.Employee.LastName);

            TypeAdapterConfig<CreateCargoDto, Cargo>.NewConfig()
                .Ignore(dest => dest.DeliveryAddressId);

            TypeAdapterConfig<Cargo, UpdateCargoDto>.NewConfig()
                .Map(dest => dest.FullAddress, src => src.DeliveryAddress != null ? src.DeliveryAddress.FullAddress : string.Empty)
                .Map(dest => dest.City, src => src.DeliveryAddress != null ? src.DeliveryAddress.City : string.Empty)
                .Map(dest => dest.District, src => src.DeliveryAddress != null ? src.DeliveryAddress.District : string.Empty)
                .Map(dest => dest.ReceiverPhone, src => src.AssignedEmployee != null ? src.Receiver.PhoneNumber : string.Empty);

            TypeAdapterConfig<AuditLog, ResultAuditLogDto>.NewConfig()
    .Map(dest => dest.UserFullName, src => src.User != null
        ? $"{src.User.FirstName} {src.User.LastName}".Trim()
        : "Sistem")
    .Map(dest => dest.OldValue, src => src.OldValue ?? "-")
    .Map(dest => dest.NewValue, src => src.NewValue ?? "-");
        }
    }
}
