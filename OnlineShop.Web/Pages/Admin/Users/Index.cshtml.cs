using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core.DTOs.Users;
using OnlineShop.Core.Services.Interfaces;

namespace OnlineShop.Web.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        IUserServices _userServices;
        public IndexModel(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public UsersFormAdminViewModel UsersViewModel { get; set; }

        public void OnGet(int pageId = 1, string filterFullName = "", string filterUserName = "", string filterEmail = "")
        {
            UsersViewModel = _userServices.GetUsers(pageId, filterFullName, filterUserName, filterEmail);
        }
    }
}
