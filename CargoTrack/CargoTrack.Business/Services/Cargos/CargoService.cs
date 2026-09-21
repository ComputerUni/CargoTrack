using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.CargosDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(ICargoRepository _repository) : ICargoService
    {
        public async Task CreateAsync(CreateCargoDto createCargoDto)
        {
            var cargo = createCargoDto.Adapt<Cargo>();
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

        public Task UpdateStatusAsync(CargoStatusUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
