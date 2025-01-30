using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class ChangePasswordCompleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChangePasswordCompleteModel> _logger;
        public ChangePasswordCompleteModel(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordCompleteModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var user = _userManager.GetUserAsync(User);
        }
    }
}
