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

        public async Task<List<ResultCargoDto>> GetFilteredDeliveredUserCargosAsync(Guid userId, string search, string status, string dateRange)
        {
            var cargos = await _userCargoRepository.GetFilteredDeliveredByUserIdAsync(userId);
            return ApplyFilters(cargos, search, status, dateRange).Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetFilteredReceivedUserCargosAsync(Guid userId, string search, string status, string dateRange)
        {
            var cargos = await _userCargoRepository.GetFilteredReceivedByUserIdAsync(userId);
            return ApplyFilters(cargos, search, status, dateRange).Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetFilteredSentUserCargosAsync(Guid userId, string search, string status, string dateRange)
        {
            var cargos = await _userCargoRepository.GetFilteredSentByUserIdAsync(userId);
            return ApplyFilters(cargos, search, status, dateRange).Adapt<List<ResultCargoDto>>();
        }

        private List<ResultCargoDto> ApplyFilters(List<Cargo> cargos, string search, string status, string dateRange)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                cargos = cargos.Where(x =>
            (!string.IsNullOrEmpty(x.TrackCode) && x.TrackCode.Contains(term, StringComparison.OrdinalIgnoreCase)) ||

            (x.Sender != null && (
                (!string.IsNullOrEmpty(x.Sender.FirstName) && x.Sender.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.Sender.LastName) && x.Sender.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                ($"{x.Sender.FirstName} {x.Sender.LastName}".Contains(term, StringComparison.OrdinalIgnoreCase))
            )) ||

            (x.Receiver != null && (
                (!string.IsNullOrEmpty(x.Receiver.FirstName) && x.Receiver.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.Receiver.LastName) && x.Receiver.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                ($"{x.Receiver.FirstName} {x.Receiver.LastName}".Contains(term, StringComparison.OrdinalIgnoreCase))
            )) ||

            (x.OriginBranch != null && !string.IsNullOrEmpty(x.OriginBranch.Name) && x.OriginBranch.Name.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
            (x.DestinationBranch != null && !string.IsNullOrEmpty(x.DestinationBranch.Name) && x.DestinationBranch.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
        ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<CargoStatus>(status, out var cargoStatus))
            {
                cargos = cargos.Where(x => x.CargoStatus == cargoStatus).ToList();
            }

            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                var cutoff = dateRange switch
                {
                    "3d" => DateTime.Now.AddDays(-3),
                    "7d" => DateTime.Now.AddDays(-7),
                    "14d" => DateTime.Now.AddDays(-14),
                    _ => DateTime.MinValue
                };

                if (cutoff != DateTime.MinValue)
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

        public async Task<List<ResultCargoDto>> GetFilteredActiveUserCargosAsync(Guid userId, string search, string status, string dateRange)
        {
            var cargos = await _userCargoRepository.GetByUserIdAsync(userId);
            return ApplyFilters(cargos, search, status, dateRange);
        }
    }
}
