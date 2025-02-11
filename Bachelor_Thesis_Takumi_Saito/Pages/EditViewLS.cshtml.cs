using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<EditViewLsModel> _logger;

        [BindProperty(SupportsGet = true, Name = "lsid")] // This binds the route parameter
        public Guid? LsId { get; set; }
        public LearningSet? LsToDisplay { get; set; }

        public EditViewLsModel(UserManager<ApplicationUser> userManager,
            AppDbContext context,
            ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("***Logging test***");

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (LsId is null)
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
        [Route("EditViewLs/UpdateTest")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]

        public JsonResult OnPostUpdate([FromBody] UpdateDto updatedData)
        {
            _logger.LogInformation("***Update Endpoint triggered***");

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


        [HttpGet]
        [Route("EditViewLs/Test")]
        public JsonResult TestEndpoint() // This worked
        {
            return new JsonResult(new { success = true, message = "Test endpoint works" });
        }

        public class UpdateDto
        {
            public Guid Id { get; set; } // ID of the LearningSet
            public string? OriginalText { get; set; }
            public string? TranslatedText { get; set; }
        }

    }
}
