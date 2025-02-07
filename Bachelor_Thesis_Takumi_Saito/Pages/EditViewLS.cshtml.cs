using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class EditViewLsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        [BindProperty(SupportsGet = true, Name ="lsid")] // This binds the route parameter
        public Guid? LsId { get; set; }
        public LearningSet? LsToDisplay { get; set; }

        public EditViewLsModel(UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); 

            if (LsId is null )
            {
                RedirectToPage("Index");
            }

            // Fetch the LearningSet using ID
            LsToDisplay = await _context.LearningSets
                .Include(ls => ls.WordMeaningPairs)
                .FirstOrDefaultAsync(ls => ls.Id == LsId);
            if (LsToDisplay == null)
            {
                RedirectToPage("List");
            }
        }
    }
}
