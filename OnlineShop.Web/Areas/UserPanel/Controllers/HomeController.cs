using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OnlineShop.Core.DTOs.Users;

namespace OnlineShop.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserServices _userServices;
        public HomeController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            return View(_userServices.GetUserInformation(User.Identity.Name));
        }

        #region Profile Edit

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            var temp = _userServices.GetUserForEditProfile(User.Identity.Name);
            return View(temp);
        }

        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            // TODO Activate Email After Edit Profile
            _userServices.EditProfile(User.Identity.Name, profile);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login?EditProfile=true");
        }

        #endregion

        #region Change Password

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string UserName = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            if(!_userServices.CompareOldPassword(UserName, changePassword.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد.");
                return View(changePassword);
            }

            _userServices.ChangePassword(UserName, changePassword.Password);
            ViewBag.Success = true;

            return View();
        }

        #endregion
    }
}
