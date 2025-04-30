using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Taccolo.Pages.Data;
using static Taccolo.Pages.EditViewLsModel;

namespace Taccolo.Pages
{
    public class FlashcardModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        [BindProperty(SupportsGet = true, Name = "lsid")] // This binds the route parameter to LsId
        public Guid? LsId { get; set; }
        public LearningSet? LsToDisplay { get; set; }
        public FlashcardModel(UserManager<ApplicationUser> userManager,
               AppDbContext context,
               ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("***Logging test Flashcard***");
            _logger.LogInformation("LsId is: " + LsId.ToString());

            if (LsId == null || LsId == Guid.Empty)
            {
                _logger.LogInformation("***LsId is NULL***");
                return RedirectToPage("Error");
            }

            // Fetch the LearningSet using ID together with associated WordMeaningPair
            LsToDisplay = await _context.LearningSets
                .Include(ls => ls.WordMeaningPairs)
                .FirstOrDefaultAsync(ls => ls.Id == LsId);

            if (LsToDisplay is null)
            {
                _logger.LogInformation("***LsToDisplay is NULL***");
                return RedirectToPage("Error");
            }
            else
            {
                _logger.LogInformation("***LsToDisplay is NOT NULL***");
                ApplicationUser? user = await _userManager.GetUserAsync(User);
            }
            return Page();
        }
    }
}
