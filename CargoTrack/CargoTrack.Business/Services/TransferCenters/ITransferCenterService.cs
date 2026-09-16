using CargoTrack.DTO.DTOs.TransferCenterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.TransferCenters
{
    public interface ITransferCenterService
    {
        Task<List<ResultTransferCenterDto>> GetAllAsync();
        Task<UpdateTransferCenterDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateTransferCenterDto createTransferCenterDto);
        Task UpdateAsync(UpdateTransferCenterDto updateTransferCenterDto);
        Task DeleteAsync(Guid id);
    }
}
