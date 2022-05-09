using AspnetBasics.Models;
using AspnetBasics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetBasics.Pages
{
    public class OnlyAdminModel : PageModel
    {
        private readonly IUserService _userService;

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> OnGetAsync()
        {
            UserInfo = await _userService.GetUserInfo();

            return Page();
        }

        public UserInfoViewModel UserInfo { get; set; } = new UserInfoViewModel(null);
    }
}
