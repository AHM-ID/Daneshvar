using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.DTOs.Users;
using OnlineShop.Core.Services.Interfaces;

namespace OnlineShop.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserServices _userServices;
        public WalletController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewData["WalletItemsList"] = _userServices.GetWalletsUser(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult? Index(WalletChargeViewModel wallet)
        {
            if (!ModelState.IsValid)
            {
                ViewData["WalletItemsList"] = _userServices.GetWalletsUser(User.Identity.Name);
                return View(wallet);
            }

            int walletID = _userServices.ChargeWallet(User.Identity.Name, wallet.Amount, "شارژ حساب");

            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(wallet.Amount);
            var response = payment.PaymentRequest("شارژ کیف پول", $"https://localhost:44359/OnlinePayment/{walletID}");
            if(response.Result.Status == 100)
            {
                return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{response.Result.Authority}");
            }

            #endregion
            return null;
        }
    }
}
