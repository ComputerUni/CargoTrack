using CargoTrack.DataAccess.Repositories.UserCargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CargoTrack.Business.Services.UserCargos
{
    public class UserCargoService(IUserCargoRepository _userCargoRepository, UserManager<AppUser> _userManager) : IUserCargoService
    {
        public async Task<ResultCargoDto> GetByIdAsync(Guid userId, Guid cargoId)
        {
            var cargo = await _userCargoRepository.GetByIdAsync(userId, cargoId);
            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task<List<ResultCargoDto>> GetByUserIdAsync(Guid userId)
        {
            var cargos = await _userCargoRepository.GetByUserIdAsync(userId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<ResultCargoDto> GetCargoDetailByCode(Guid userId, string trackCode)
        {
            var cargo = await _userCargoRepository.GetCargoDetailByCode(userId, trackCode);
            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task<List<ResultCargoDto>> GetDeliveredByUserIdAsync(Guid userId)
        {
            var cargos = await _userCargoRepository.GetDeliveredByUserIdAsync(userId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetFilteredUserCargosAsync(Guid userId, string search, string status, string dateRange, bool onlyActive = false, bool onlyDelivered = false, bool onlySent = false, bool onlyReceived = false)
        {
            var cargos = await _userCargoRepository.GetByUserIdAsync(userId);
            if (onlyActive)
            {
                cargos = cargos.Where(x => x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender).ToList();
            }

            if (onlyDelivered)
            {
                cargos = cargos.Where(x => x.CargoStatus == CargoStatus.Delivered).ToList();
            }

            if (onlySent)
            {
                cargos = cargos.Where(x => x.SenderId == userId).ToList();
            }

            if (onlyReceived)
            {
                cargos = cargos.Where(x => x.ReceiverId == userId).ToList();
            }

            if(!string.IsNullOrWhiteSpace(search))
            {
                cargos = cargos.Where(x => x.TrackCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           x.Sender.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           x.Sender.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           x.Receiver.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           x.Receiver.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if(!string.IsNullOrWhiteSpace(status) && Enum.TryParse<CargoStatus>(status, out var cargoStatus))
            {
                cargos = cargos.Where(x => x.CargoStatus == cargoStatus).ToList();
            }

            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                var cutoff = dateRange switch
                {
                    "30d" => DateTime.Now.AddDays(-30),
                    "90d" => DateTime.Now.AddDays(-90),
                    "this year" => new DateTime(DateTime.Now.Year, 1, 1),
                    _ => DateTime.MinValue
                };

                if(cutoff != DateTime.MinValue)
                {
                    cargos = cargos.Where(x => x.CreatedDate >= cutoff).ToList();
                }
            }

            return cargos.Adapt<List<ResultCargoDto>>();

        }

        public async Task<List<ResultCargoDto>> GetReceivedByUserIdAsync(Guid userId)
        {
            var cargos = await _userCargoRepository.GetReceivedByUserIdAsync(userId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetSentByUserIdAsync(Guid userId)
        {
            var cargos = await _userCargoRepository.GetSentByUserIdAsync(userId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }
    }
}
