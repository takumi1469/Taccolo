using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class TestModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public TestModel(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); //Gets user information from DI
        }
    }
}
