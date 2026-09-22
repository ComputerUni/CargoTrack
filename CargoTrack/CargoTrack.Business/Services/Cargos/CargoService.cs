using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(ICargoRepository _repository, ICargoPricingService _cargoPricingService, IBranchRepository _branchRepository) : ICargoService
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

            var cargo = new Cargo
            {
                TrackCode = trackCode,
                ShipmentDate = DateTime.Now,
                EstimatedArrivalDate = estimatedDate,
                Weight = createCargoDto.Weight,
                Length = createCargoDto.Length,   
                Width = createCargoDto.Width,     
                Height = createCargoDto.Height,   
                Desi = desi,
                Price = price,
                CargoType = createCargoDto.CargoType,
                CargoStatus = CargoStatus.Created,
                SenderId = createCargoDto.SenderId,
                ReceiverId = createCargoDto.ReceiverId,
                OriginBranchId = createCargoDto.OriginBranchId,
                DestinationBranchId = createCargoDto.DestinationBranchId
            };

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
            var cargo = await _repository.GetByIdAsync(id);
            if(cargo is null)
            {
                throw new Exception("Cargo Not Found");
            }
            return cargo.Adapt<UpdateCargoDto>();
        }

        public async Task<ResultCargoDto> GetByTrackCodeAsync(string trackCode)
        {
            var cargo = await _repository.GetByTrackCodeAsync(trackCode);
            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task UpdateAsync(UpdateCargoDto updateCargoDto)
        {
            var cargo = updateCargoDto.Adapt<Cargo>();
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


        public Task UpdateStatusAsync(CargoStatusUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
