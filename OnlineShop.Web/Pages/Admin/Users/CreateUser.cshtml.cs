using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Core.DTOs.Users;

namespace OnlineShop.Web.Pages.Admin.Users
{
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        public CreateUserViewModel createUser { get; set; }
        public void OnGet()
        {
        }
    }
}
