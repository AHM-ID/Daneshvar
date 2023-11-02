using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.Services.Interfaces;
using OnlineShop.Web.Models;
using System.Diagnostics;

namespace OnlineShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserServices _userServices;

        public HomeController(ILogger<HomeController> logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            // TODO AutoComplete Search Box
            return View();
        }


        #region Payment

        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var wallet = _userServices.GetWalletByWalletID(id);
                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var response = payment.Verification(authority).Result;
                if(response.Status == 100)
                {
                    ViewBag.TrackCode = response.RefId;
                    ViewBag.Success = true;
                    wallet.IsPaid = true;
                    _userServices.WalletBalance(User.Identity.Name, wallet.Amount, 1, wallet.IsPaid);
                    _userServices.UpdateWallet(wallet);
                }
            }
            return View();
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}