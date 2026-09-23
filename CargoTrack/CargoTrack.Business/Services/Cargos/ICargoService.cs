using CargoTrack.DTO.DTOs.CargosDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Cargos
{
    public interface ICargoService
    {
        Task<List<ResultCargoDto>> GetAllAsync();
        Task<UpdateCargoDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateCargoDto createCargoDto);
        Task UpdateAsync(UpdateCargoDto updateCargoDto);
        Task DeleteAsync(Guid id);
        Task<ResultCargoDto> GetByTrackCodeAsync(string trackCode);
        Task UpdateStatusAsync(CargoStatusUpdateDto dto);
        Task<List<ResultCargoDto>> GetByBranchIdAsync(Guid branchId);
        Task<ResultCargoDto> GetByIdWithDetailsAsync(Guid id);
    }
}
