using CargoTrack.Business.Services.CargoMovements;
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Addresses;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(IAddressRepository _addressRepository, IDeliveryExceptionService _deliveryExceptionService, ICargoRepository _repository, ICargoPricingService _cargoPricingService, IBranchRepository _branchRepository, ICargoMovementService _cargoMovementService, IDeliveryService _deliveryService, UserManager<AppUser> _userManager) : ICargoService
    {
        public async Task CreateAsync(CreateCargoDto createCargoDto)
        {
            var desi = (createCargoDto.Length * createCargoDto.Width * createCargoDto.Height) / 3000;

            var originBranch = await _branchRepository.GetByIdAsync(createCargoDto.OriginBranchId);
            var destinationBranch = await _branchRepository.GetByIdAsync(createCargoDto.DestinationBranchId);
            var isIntercity = originBranch.CityId != destinationBranch.CityId;

            var price = await _cargoPricingService.CalculatePriceAsync(createCargoDto.Weight, desi, createCargoDto.CargoType, isIntercity);
            var estimatedDate = await _cargoPricingService.CalculateEstimatedDeliveryDateAsync(createCargoDto.OriginBranchId, createCargoDto.DestinationBranchId, createCargoDto.CargoType);
            var trackCode = await _cargoPricingService.GenerateTrackCode();

            var cargo = createCargoDto.Adapt<Cargo>();

            var address = new Address
            {
                Title = "Teslimat Adresi",
                FullAddress = createCargoDto.FullAddress,
                District = createCargoDto.District,
                City = createCargoDto.City,
                UserId = createCargoDto.ReceiverId
            };

            await _addressRepository.CreateAsync(address);

            if(!string.IsNullOrWhiteSpace(createCargoDto.ReceiverPhone))
            {
                var receiverUser = await _userManager.FindByIdAsync(createCargoDto.ReceiverId.ToString());
                if(receiverUser != null)
                {
                    receiverUser.PhoneNumber = createCargoDto.ReceiverPhone;
                    await _userManager.UpdateAsync(receiverUser);
                }
            }

            cargo.TrackCode = trackCode;
            cargo.ShipmentDate = DateTime.Now;
            cargo.EstimatedArrivalDate = estimatedDate;
            cargo.Desi = desi;
            cargo.Price = price;
            cargo.CargoStatus = CargoStatus.Created;
            cargo.DeliveryAddressId = address.Id;

            await _repository.CreateAsync(cargo);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cargo = await _repository.GetByIdAsync(id);
            if (cargo is null)
            {
                throw new ValidationException("Cargo Not Found");
            }
            await _repository.DeleteAsync(cargo);
        }

        public async Task<List<ResultCargoDto>> GetAllAsync()
        {
            var cargos = await _repository.GetAllWithDetailsAsync();
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<UpdateCargoDto> GetByIdAsync(Guid id)
        {
            var cargo = await _repository.GetByIdWithDetailsAsync(id);
            if (cargo is null)
            {
                throw new Exception("Cargo Not Found");
            }

            var dto = cargo.Adapt<UpdateCargoDto>();

            if(cargo.Receiver != null)
            {
                dto.ReceiverPhone = cargo.Receiver.PhoneNumber;
            }

            return dto;
        }

        public async Task<ResultCargoDto> GetByTrackCodeAsync(string trackCode)
        {
            var cargo = await _repository.GetByTrackCodeAsync(trackCode);
            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task UpdateAsync(UpdateCargoDto updateCargoDto)
        {
            var cargo = await _repository.GetByIdWithDetailsAsync(updateCargoDto.Id);
            if (cargo == null)
            {
                throw new Exception("Kargo bulunamadı");
            }

            cargo.Weight = updateCargoDto.Weight;
            cargo.Length = updateCargoDto.Length;
            cargo.Width = updateCargoDto.Width;
            cargo.Height = updateCargoDto.Height;
            cargo.CargoType = updateCargoDto.CargoType;
            cargo.SenderId = updateCargoDto.SenderId;
            cargo.ReceiverId = updateCargoDto.ReceiverId;
            cargo.OriginBranchId = updateCargoDto.OriginBranchId;
            cargo.DestinationBranchId = updateCargoDto.DestinationBranchId;

            var desi = (updateCargoDto.Length * updateCargoDto.Width * updateCargoDto.Height) / 3000;

            cargo.Desi = desi;


            var originBranch = await _branchRepository.GetByIdAsync(updateCargoDto.OriginBranchId);
            var destinationBranch = await _branchRepository.GetByIdAsync(updateCargoDto.DestinationBranchId);
            var isIntercity = originBranch.CityId != destinationBranch.CityId;

            cargo.Price = await _cargoPricingService.CalculatePriceAsync(updateCargoDto.Weight, desi, updateCargoDto.CargoType, isIntercity);

            if(cargo.DeliveryAddress != null)
            {
                cargo.DeliveryAddress.FullAddress = updateCargoDto.FullAddress;
                cargo.DeliveryAddress.City = updateCargoDto.City;
                cargo.DeliveryAddress.District = updateCargoDto.District;
                cargo.DeliveryAddress.UserId = updateCargoDto.ReceiverId;
                await _addressRepository.UpdateAsync(cargo.DeliveryAddress);
            }
            else
            {
                var newAddress = new Address
                {
                    Title = "Teslimat Adresi",
                    FullAddress = updateCargoDto.FullAddress,
                    City = updateCargoDto.City,
                    District = updateCargoDto.District,
                    UserId = updateCargoDto.ReceiverId
                };
                await _addressRepository.CreateAsync(newAddress);
                cargo.DeliveryAddressId = newAddress.Id;
            }

            if(cargo.Receiver != null && !string.IsNullOrWhiteSpace(updateCargoDto.ReceiverPhone))
            {
                cargo.Receiver.PhoneNumber = updateCargoDto.ReceiverPhone;
                await _userManager.UpdateAsync(cargo.Receiver);
            }

            await _repository.UpdateAsync(cargo);

        }

        public async Task<List<ResultCargoDto>> GetByBranchIdAsync(Guid branchId)
        {
            var cargos = await _repository.GetByBranchIdAsync(branchId);
            return cargos.Select(x => new ResultCargoDto
            {
                Id = x.Id,
                TrackCode = x.TrackCode,
                ShipmentDate = x.ShipmentDate,
                EstimatedArrivalDate = x.EstimatedArrivalDate,
                Weight = x.Weight,
                Desi = x.Desi,
                Price = x.Price,
                CargoStatus = x.CargoStatus,
                CargoType = x.CargoType,
                SenderName = x.Sender.FirstName + " " + x.Sender.LastName,
                ReceiverName = x.Receiver.FirstName + " " + x.Receiver.LastName,
                OriginBranchName = x.OriginBranch.Name,
                DestinationBranchName = x.DestinationBranch.Name
            }).ToList();
        }


        public async Task UpdateStatusAsync(CargoStatusUpdateDto dto)
        {
            var cargo = await _repository.GetByIdAsync(dto.Id);

            if (cargo is null)
            {
                throw new ValidationException("Böyle bir kargo bulumamadı");
            }

            if (dto.NewStatus == CargoStatus.DeliveryFailed)
            {
                cargo.FailedAttemptCount++;
                await _deliveryExceptionService.RecordDeliveryExceptionAsync(dto.Id, dto.EmployeeId.Value, dto.ExceptionReason.Value, cargo.FailedAttemptCount, dto.Description);
                await _repository.UpdateAsync(cargo);
                await _cargoMovementService.CreateMovementAsync(dto.Id, dto.NewStatus, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);

                if (cargo.FailedAttemptCount >= 3)
                {
                    await _cargoMovementService.CreateMovementAsync(dto.Id, CargoStatus.ReturnInProcess, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);
                }

                return;
            }

            await _cargoMovementService.CreateMovementAsync(dto.Id, dto.NewStatus, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);


            if (dto.NewStatus == CargoStatus.OutForDelivery)
            {
                await _deliveryService.GenerateDeliveryCodeAsync(dto.Id);
            }
        }

        public async Task<ResultCargoDto> GetByIdWithDetailsAsync(Guid id)
        {
            var cargo = await _repository.GetByIdWithDetailsAsync(id);
            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task<List<ResultCargoDto>> GetOutDeliveryByBranchIdAsync(Guid branchId)
        {
            var cargo = await _repository.GetOutDeliveryByBranchIdAsync(branchId);
            return cargo.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetDeliveryFailedOrReturnInProcessByBranchIdAsync(Guid branchId)
        {
            var cargo = await _repository.GetDeliveryFailedOrReturnInProcessByBranchIdAsync(branchId);
            return cargo.Adapt<List<ResultCargoDto>>();
        }
    }
}
