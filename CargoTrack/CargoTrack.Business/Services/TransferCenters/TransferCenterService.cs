using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.DTO.DTOs.TransferCenterDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.TransferCenters
{
    public class TransferCenterService(ITransferCenterRepository _repository) : ITransferCenterService
    {
        public async Task CreateAsync(CreateTransferCenterDto createTransferCenterDto)
        {
            var transferCenter = createTransferCenterDto.Adapt<TransferCenter>();
            await _repository.CreateAsync(transferCenter);
        }

        public async Task DeleteAsync(Guid id)
        {
            var transferCenter = await _repository.GetByIdAsync(id);
            if(transferCenter is null)
            {
                throw new ValidationException("Transfer Center Not Found");
            }
            await _repository.DeleteAsync(transferCenter);
        }

        public async Task<List<ResultTransferCenterDto>> GetAllAsync()
        {
            var transferCenters = await _repository.GetAllAsync();
            return transferCenters.Adapt<List<ResultTransferCenterDto>>();
        }

        public async Task<UpdateTransferCenterDto> GetByIdAsync(Guid id)
        {
            var transferCenter = await _repository.GetByIdAsync(id);
            if(transferCenter is null)
            {
                throw new ValidationException("Transfer Center Not Found");
            }
            return transferCenter.Adapt<UpdateTransferCenterDto>();
        }

        public async Task UpdateAsync(UpdateTransferCenterDto updateTransferCenterDto)
        {
            var transferCenter = updateTransferCenterDto.Adapt<TransferCenter>();
            await _repository.UpdateAsync(transferCenter);
        }
    }
}
