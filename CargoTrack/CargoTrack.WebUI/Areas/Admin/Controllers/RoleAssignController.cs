using CargoTrack.Business.Services.Branches;
using CargoTrack.DTO.DTOs.UserDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Area.Admin)]
    public class RoleAssignController(UserManager<AppUser> _userManager, RoleManager<AppRole> _roleManager, IBranchService _branchService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var mappedUsers = users.Adapt<List<ResultUserDto>>();

            foreach (var item in mappedUsers)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
                item.Roles = await _userManager.GetRolesAsync(user);
            }
            return View(mappedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserForRoleAssign(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleAssignList = new List<RoleAssingDto>();

            ViewBag.fullName = string.Join(" ", user.FirstName, user.LastName);
            ViewBag.Branches = new SelectList(await _branchService.GetAllAsync(), "Id", "Name");
            ViewBag.CurrentBranchId = user.BranchId;

            foreach (var role in roles)
            {
                roleAssignList.Add(new RoleAssingDto
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleExist = userRoles.Contains(role.Name)
                });
            }

            return View(roleAssignList);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserForRoleAssign(List<RoleAssingDto> model, Guid? branchId)
        {
            var userId = model.Select(x => x.UserId).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            user.BranchId = branchId;
            await _userManager.UpdateAsync(user);

            foreach(var assignRole in model)
            {
                if(assignRole.RoleExist)
                {
                    await _userManager.AddToRoleAsync(user, assignRole.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, assignRole.RoleName);
                }
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
