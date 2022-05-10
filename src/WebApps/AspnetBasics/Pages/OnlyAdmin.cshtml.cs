using AspnetBasics.Models;
using AspnetBasics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetBasics.Pages
{
    [Authorize(Roles = "admin")]
    public class OnlyAdminModel : PageModel
    {
        private readonly IUserService _userService;

        public OnlyAdminModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        public UserInfoViewModel UserInfo { get; set; } = new UserInfoViewModel(null);

        public async Task<IActionResult> OnGetAsync()
        {
            UserInfo = await _userService.GetUserInfo();

            return Page();
        }        
    }
}
