using CargoTrack.DataAccess.Repositories.UserCargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.UserCargos
{
    public class UserCargoService(IUserCargoRepository _userCargoRepository) : IUserCargoService
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

        public async Task<List<ResultCargoDto>> GetDeliveredByUserIdAsync(Guid userId)
        {
            var cargos = await _userCargoRepository.GetDeliveredByUserIdAsync(userId);
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
