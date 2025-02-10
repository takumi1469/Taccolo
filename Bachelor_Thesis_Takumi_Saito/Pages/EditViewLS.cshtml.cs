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

        [HttpPost]
        [Route("EditViewLs/UpdateLs/{lsid}")]
        public JsonResult OnPostUpdateLs([FromBody] UpdateLsDto updatedData)
        {
            var currentLs = _context.LearningSets.Find(updatedData.Id);

            if (currentLs == null)
            {
                return new JsonResult(new { success = false, message = "LearningSet not found" });
            }

            // Update properties based on the incoming data
            currentLs.Input = updatedData.OriginalText;
            currentLs.Translation = updatedData.TranslatedText;

            // Save changes to the database
            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully" });
        }

        public class UpdateLsDto
        {
            public Guid Id { get; set; } // ID of the LearningSet
            public string? OriginalText { get; set; }
            public string? TranslatedText { get; set; }
        }

    }
}
