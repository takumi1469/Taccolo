using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Taccolo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); //Gets user information from DI
        }

        public async Task<IActionResult> OnGetLogoutAsync()
{
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Get back to top after logging out
        }
    }
}
