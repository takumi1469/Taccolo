using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Taccolo.Areas.Identity.Pages.Account
{
    public class MyPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MyPageModel> _logger;
        public ApplicationUser? CurrentUser { get; set; }   

        public MyPageModel(UserManager<ApplicationUser> userManager, ILogger<MyPageModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User); 
        }

    }
}
