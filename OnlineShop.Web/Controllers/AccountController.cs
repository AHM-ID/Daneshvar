using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using OnlineShop.Core.Convertors;
using OnlineShop.Core.DTOs.Users;
using OnlineShop.Core.Generators;
using OnlineShop.Core.Security;
using OnlineShop.Core.Senders;
using OnlineShop.Core.Services;
using OnlineShop.Core.Services.Interfaces;
using OnlineShop.DataLayer.Entities.User;
using System.Security.Claims;

namespace OnlineShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserServices _userServices;
        private IViewRenderService _viewService;
        public AccountController(IUserServices userServices, IViewRenderService viewService)
        {
            _userServices = userServices;
            _viewService = viewService;
        }

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userServices.IsUserNameExist(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری قبلا انتخاب شده است.");
                return View(register);
            }

            if (_userServices.IsEmailExist(StringExtensions.EmailFix(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل قبلا ثبت نام کرده است.");
                return View(register);
            }

            User user = new User()
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Gender = register.Gender,
                Email = StringExtensions.EmailFix(register.Email),
                Password = PasswordSecurity.HashPassword(register.Password),
                ActivationCode = UserCodeGenerator.UniqueCodeGenerator(),
                IsActive = false,
                RegisterDate = DateTime.Now
            };

            if (register.Gender == Gender.Male)
                user.UserAvatar = "Male.png";
            else
                user.UserAvatar = "Female.png";

            _userServices.AddUser(user);

            #region Send Activation Email

            // TODO Fix Email Name and Logo
            string body = _viewService.RenderToStringAsync("_EmailActivation", user);
            SendEmail.SendAsync(user.Email, $"{user.FullName} عزیز خوش آمدی !", body);

            #endregion

            return View("SuccessRegistration", user);
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userServices.LoginUser(login);

            if (user != null)
            {
                if (user.IsActive)
                {
                    ViewBag.Success = true;

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("FullName", user.FullName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, properties);

                    return View();
                }
                else
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد.");
            }

            ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده وجود ندارد.");

            return View(login);
        }

        #endregion

        #region Logout

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

        #region Activate Account

        public IActionResult ActiveAccount(string Id)
        {
            ViewBag.IsActive = _userServices.ActiveAccount(Id);
            return View();
        }

        #endregion

        #region Forgot Password

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if(!ModelState.IsValid)
            {
                return View(forgot);
            }

            string FixEmail = StringExtensions.EmailFix(forgot.Email);
            User user = _userServices.GetUserByEmail(FixEmail);

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد.");
                return View(forgot);
            }

            string emailBody = _viewService.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.SendAsync(user.Email, "بازیابی رمز عبور", emailBody);

            ViewBag.Sent = true;
            return View();
        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActivationCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            User user = _userServices.GetUserByActivationCode(reset.ActivationCode);
            if(user == null)
            {
                return NotFound();
            }

            user.Password = PasswordSecurity.HashPassword(reset.Password);
            _userServices.UpdateUser(user);

            return RedirectToAction("Login");
        }

        #endregion
    }
}
