using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Business.Services.Employees
{
    public class EmployeeService(IEmployeeRepository _repository) : IEmployeeService
    {
        public async Task CreateAsync(CreateEmployeeDto createEmployeeDto)
        {
            var employee = createEmployeeDto.Adapt<Employee>();
            await _repository.CreateAsync(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null)
            {
                throw new ValidationException("Employee Not Found");
            }
            await _repository.DeleteAsync(employee);
        }

        public async Task<List<ResultEmployeeDto>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.Adapt<List<ResultEmployeeDto>>();
        }

        public async Task<List<ResultEmployeeDto>> GetByBranchIdAsync(Guid branchId)
        {
            var employees = await _repository.GetByBranchIdAsync(branchId);
            return employees.Select(x => new ResultEmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                BranchId = x.BranchId,
                BranchName = x.Branch.Name
            }).ToList();
        }

        public async Task<UpdateEmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null)
            {
                throw new ValidationException("Employee Not Found");
            }
            return employee.Adapt<UpdateEmployeeDto>();
        }

        public async Task UpdateAsync(UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = updateEmployeeDto.Adapt<Employee>();
            await _repository.UpdateAsync(employee);
        }
    }
}
