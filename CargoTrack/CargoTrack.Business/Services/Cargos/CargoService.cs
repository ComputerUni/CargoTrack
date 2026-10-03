using CargoTrack.Business.Extensions;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.CargoMovements;
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.Business.Services.Deliveries;
using CargoTrack.Business.Services.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Addresses;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.DTO.DTOs.ManagerCargoDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(IAuditLogService _auditLogService, IAddressRepository _addressRepository, IDeliveryExceptionService _deliveryExceptionService, ICargoRepository _repository, ICargoPricingService _cargoPricingService, IBranchRepository _branchRepository, ICargoMovementService _cargoMovementService, IDeliveryService _deliveryService, UserManager<AppUser> _userManager) : ICargoService
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

            if (!string.IsNullOrWhiteSpace(createCargoDto.ReceiverPhone))
            {
                var receiverUser = await _userManager.FindByIdAsync(createCargoDto.ReceiverId.ToString());
                if (receiverUser != null)
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

            var actionUserId = createCargoDto.CurrentUserId ?? createCargoDto.SenderId;

            await _auditLogService.CreateAuditLogAsync(
                userId: actionUserId,
                entityId: cargo.Id,
                actionType: "Create",
                entityName: "Cargo",
                description: $"{cargo.TrackCode} takip numaralı kargo sisteme kaydedildi",
                oldValue: null,
                newValue: CargoStatus.Created.GetDisplayName()
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            var cargo = await _repository.GetByIdAsync(id);
            if (cargo is null)
            {
                throw new ValidationException("Cargo Not Found");
            }
            cargo.IsDeleted = true;
            await _repository.UpdateAsync(cargo);
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

            if (cargo.Receiver != null)
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

            if (cargo.DeliveryAddress != null)
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

            if (cargo.Receiver != null && !string.IsNullOrWhiteSpace(updateCargoDto.ReceiverPhone))
            {
                cargo.Receiver.PhoneNumber = updateCargoDto.ReceiverPhone;
                await _userManager.UpdateAsync(cargo.Receiver);
            }

            await _repository.UpdateAsync(cargo);

        }

        public async Task<List<ResultCargoDto>> GetByBranchIdAsync(Guid branchId)
        {
            var cargos = await _repository.GetByBranchIdAsync(branchId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }


        public async Task UpdateStatusAsync(CargoStatusUpdateDto dto)
        {
            var cargo = await _repository.GetByIdWithMovementAsync(dto.Id);

            if (cargo is null)
            {
                throw new ValidationException("Böyle bir kargo bulumamadı");
            }

            var oldStatus = cargo.CargoStatus;

            if (dto.BranchId.HasValue)
            {
                var lastMovement = cargo.CargoMovements.OrderByDescending(m => m.MovementDate).FirstOrDefault();

                bool isAtMyBranch = cargo.CargoMovements.Any() ? lastMovement?.BranchId == dto.BranchId : cargo.OriginBranchId == dto.BranchId;
                bool isReturnComing = (cargo.FailedAttemptCount >= 3 || cargo.CargoStatus == CargoStatus.ReturnInProcess || cargo.CargoStatus == CargoStatus.InTransferCenter) && cargo.OriginBranchId == dto.BranchId;
                bool isComing = !isAtMyBranch && (cargo.DestinationBranchId == dto.BranchId || isReturnComing);

                if (!isAtMyBranch && !isComing)
                {
                    throw new UnauthorizedAccessException("Bu kargo şu anda şubenizde değil");
                }
                if (isComing && dto.NewStatus != CargoStatus.ArrivedAtDeliveryBranch && dto.NewStatus != CargoStatus.AtOriginBranch && dto.NewStatus != CargoStatus.ReturnedToSender)
                {
                    throw new UnauthorizedAccessException("Bu kargo henüz şubenize ulaşmadı, sadece teslim alabilirsiniz.");
                }
            }

            if (dto.NewStatus == CargoStatus.DeliveryFailed)
            {
                cargo.FailedAttemptCount++;
                await _deliveryExceptionService.RecordDeliveryExceptionAsync(dto.Id, dto.EmployeeId.Value, dto.ExceptionReason.Value, cargo.FailedAttemptCount, dto.Description);
                await _repository.UpdateAsync(cargo);
                await _cargoMovementService.CreateMovementAsync(dto.Id, dto.NewStatus, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);

                var actionUserId = dto.CurrentUserId ?? cargo.SenderId;

                if (dto.EmployeeId.HasValue)
                {
                    await _auditLogService.CreateAuditLogAsync(
                        userId: actionUserId,
                        entityId: cargo.Id,
                        actionType: "Durum Değişikliği",
                        entityName: "Cargo",
                        description: $"{cargo.TrackCode} takip nolu kargonun teslimatı başarısız oldu. Neden: {dto.Description}",
                        oldValue: oldStatus.ToString(),
                        newValue: dto.NewStatus.ToString()
                    );
                }

                if (cargo.FailedAttemptCount >= 3)
                {
                    await _cargoMovementService.CreateMovementAsync(dto.Id, CargoStatus.ReturnInProcess, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);
                }

                return;
            }

            await _cargoMovementService.CreateMovementAsync(dto.Id, dto.NewStatus, dto.BranchId, dto.TransferCenterId, dto.EmployeeId, dto.Description);

            var updateActionUserId = dto.CurrentUserId ?? cargo.SenderId;

            if (dto.EmployeeId.HasValue)
            {
                await _auditLogService.CreateAuditLogAsync(
                    userId: updateActionUserId,
                    entityId: cargo.Id,
                    actionType: "StatusChange",
                    entityName: "Cargo",
                    description: $"{cargo.TrackCode} takip nolu kargo durumu güncellendi: {dto.Description}",
                    oldValue: oldStatus.ToString(),
                    newValue: dto.NewStatus.ToString()
                );
            }


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

        public async Task<List<ResultCargoDto>> GetIncomingCargosAsync(Guid branchId)
        {
            var cargos = await _repository.GetIncomingCargoAsync(branchId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetOutgoingCargosAsync(Guid branchId)
        {
            var cargos = await _repository.GetOutgoingCargoAsync(branchId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<ManagerCargoIndexDto> GetBranchCargoSummaryAsync(Guid branchId)
        {
            var cargos = await _repository.GetByBranchIdAsync(branchId);
            return new ManagerCargoIndexDto
            {
                TotalCargos = cargos.Count,
                OutOfDelivery = cargos.Count(x => x.CargoStatus == CargoStatus.OutForDelivery),
                ArrivedAtBranch = cargos.Count(x => x.CargoStatus == CargoStatus.ArrivedAtDeliveryBranch),
                AtOriginBranch = cargos.Count(x => x.CargoStatus == CargoStatus.AtOriginBranch),
                DeliveryFailed = cargos.Count(x => x.CargoStatus == CargoStatus.DeliveryFailed),
                Delivered = cargos.Count(x => x.CargoStatus == CargoStatus.Delivered)
            };
        }

        public async Task<IncomingCargoSummaryDto> GetIncomingCargoSummaryAsync(Guid branchId)
        {
            var cargos = await _repository.GetIncomingCargoAsync(branchId);

            return new IncomingCargoSummaryDto
            {
                TotalIncoming = cargos.Count,
                WaitingAtBranch = cargos.Count(x => x.CargoStatus == CargoStatus.ArrivedAtDeliveryBranch),
                DeliveredToday = cargos.Count(x => x.CargoStatus == CargoStatus.Delivered && x.Delivery != null && x.Delivery.DeliveryDate.Date == DateTime.Today),
                Delayed = cargos.Count(x => x.EstimatedArrivalDate < DateTime.Now && x.CargoStatus != CargoStatus.Delivered)
            };
        }

        public async Task<OutgoingCargoSummaryDto> GetOutgoingCargoSummaryAsync(Guid branchId)
        {
            var cargos = await _repository.GetOutgoingCargoAsync(branchId);
            return new OutgoingCargoSummaryDto
            {
                TotalOutgoing = cargos.Count,
                InTransferCenter = cargos.Count(x => x.CargoStatus == CargoStatus.InTransferCenter),
                ReturnedToSender = cargos.Count(x => x.CargoStatus == CargoStatus.ReturnedToSender),
                OutOfDelivery = cargos.Count(x => x.CargoStatus == CargoStatus.OutForDelivery)
            };
        }
    }
}
