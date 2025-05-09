using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taccolo.Areas.Identity.Pages.Account;
using Taccolo.Pages.Data;

namespace Taccolo.Pages
{
    public class UserPageModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MyPageModel> _logger;
        private readonly AppDbContext _context;
        [BindProperty(SupportsGet = true, Name = "slug")]
        public string? CurrentSlug { get; set; }
        public ApplicationUser? UserToDisplay { get; set; }

        public UserPageModel(UserManager<ApplicationUser> userManager, ILogger<MyPageModel> logger, AppDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            UserToDisplay = _context.Users.FirstOrDefault(u => u.PublicSlug == CurrentSlug);
        }
    }
}
