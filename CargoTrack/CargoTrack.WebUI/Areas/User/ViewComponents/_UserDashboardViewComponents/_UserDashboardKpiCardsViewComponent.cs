using CargoTrack.Business.Services.UserCargos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.ViewComponents._UserDashboardViewComponents
{
    public class _UserDashboardKpiCardsViewComponent(UserManager<AppUser> _userManager, IUserCargoService _userCargoService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var activeCargos = await _userCargoService.GetByUserIdAsync(user.Id);
            ViewBag.ActiveCargoCount = activeCargos.Count;

            var outOfDeliveryCargos = activeCargos.Where(x => x.ReceiverId == user.Id && x.CargoStatus == CargoStatus.OutForDelivery);
            ViewBag.OutOfDelivery = outOfDeliveryCargos.Count();

            var inTransferCenterCargos = activeCargos.Where(x => x.ReceiverId == user.Id && x.CargoStatus == CargoStatus.InTransferCenter);
            ViewBag.InTransferCenter = inTransferCenterCargos.Count();

            var receivedCargos = activeCargos.Where(x => x.ReceiverId == user.Id);
            ViewBag.ReceivedCargos = receivedCargos.Count();

            var sentCargos = activeCargos.Where(x => x.SenderId == user.Id);
            ViewBag.SentCargos = sentCargos.Count();

            var completedCargos = activeCargos.Where(x => x.SenderId == user.Id || x.ReceiverId == user.Id && x.CargoStatus == CargoStatus.Delivered);
            ViewBag.CompletedCargos = completedCargos.Count();

            return View();
        }
    }
}
