using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.UserCargos
{
    public interface IUserCargoService
    {
        Task<List<ResultCargoDto>> GetByUserIdAsync(Guid userId);
        Task<List<ResultCargoDto>> GetDeliveredByUserIdAsync(Guid userId);
        Task<List<ResultCargoDto>> GetReceivedByUserIdAsync(Guid userId);
        Task<List<ResultCargoDto>> GetSentByUserIdAsync(Guid userId);
        Task<ResultCargoDto> GetByIdAsync(Guid userId, Guid cargoId);
        Task<ResultCargoDto> GetCargoDetailByCode(Guid userId, string trackCode);
        Task<List<ResultCargoDto>> GetFilteredUserCargosAsync(Guid userId, string search, string status, string dateRange, bool onlyActive = false, bool onlyDelivered = false, bool onlySent = false, bool onlyReceived = false);
    }
}
