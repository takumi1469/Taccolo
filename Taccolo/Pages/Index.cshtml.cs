using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Taccolo.Pages.Data;
using Taccolo.Pages.Shared;

namespace Taccolo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public List<LearningSet> AllLearningSets { get; set; } = new List<LearningSet>();

        [FromQuery]
        public string? Keyword { get; set; }

        public IndexModel(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
        }

        //public IActionResult OnPostSearch()
        //{
        //    if (!string.IsNullOrEmpty(Keyword))
        //    {
        //        _logger.LogInformation($"***OnPostSearch is called, Keyword is {Keyword}***");
        //        return RedirectToPage("Index", new { keyword = Keyword });
        //    }
        //    return Page();
        //}

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
