using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class MyPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MyPageModel> _logger;

        public MyPageModel(UserManager<ApplicationUser> userManager, ILogger<MyPageModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
