using CargoTrack.Business.Services.Cargos;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.AdminBranchDtos;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.ViewComponents._AdminBranchViewComponents
{
    public class _AdminBranchSummaryCardsViewComponent(IBranchRepository _branchRepository, IEmployeeRepository _employeeRepository, ICargoRepository _cargoRepository) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var branches = await _branchRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();
            var cargos = await _cargoRepository.GetAllAsync();

            var totalBranches = branches.Count;
            var activeBranches = branches.Count;
            var totalEmployees = employees.Count;

            var activeCargos = cargos.Count(x => x.CargoStatus != CargoStatus.Delivered && x.CargoStatus != CargoStatus.ReturnedToSender);

            var model = new AdminBranchSummaryDto
            {
                TotalBranches = totalBranches,
                ActiveBranches = activeBranches,
                TotalEmployees = totalEmployees,
                AvgEmployeePerBranch = totalBranches > 0 ? Math.Round((double)totalEmployees / totalBranches, 1) : 0,
                ActiveCargos = activeCargos
            };

            return View(model);
        }
    }
}
