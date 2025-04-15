using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taccolo.Pages.Data;

namespace Taccolo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public List<LearningSet> AllLearningSets { get; set; } = new List<LearningSet>();

        public IndexModel(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            AllLearningSets = _context.LearningSets.ToList();
            AllLearningSets.Reverse();
        }
    }
}
