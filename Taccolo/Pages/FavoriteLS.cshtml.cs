using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Taccolo.Pages.Data;
using Taccolo.Pages.Shared;

namespace Taccolo.Pages
{
    public class FavoriteLSModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public List<LearningSet> FavoriteLearningSets { get; set; } = new List<LearningSet>();

        public FavoriteLSModel(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); //Gets user information from DI
            if (user == null) { }
            else
            {               
                FavoriteLearningSets = _context.FavoriteSets
                    .Where(fs => fs.UserId == user.Id)
                    .Include(fs => fs.LearningSet) // Fetching the associated Learning Sets thanks to navigation property
                    .Select(fs => fs.LearningSet) // Transforming each item (FavoriteSet) to the associated Learning Set
                    .ToList();

                FavoriteLearningSets.Reverse();
            }
        }
    }
}
