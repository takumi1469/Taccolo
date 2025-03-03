using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Bachelor_Thesis_Takumi_Saito.Pages.EditViewLsModel;

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

        public List<CommentWithUsername> CommentWithUsernames { get; set; } = new List<CommentWithUsername>();



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

            // Fetch the LearningSet using ID together with associated WordMeaningPair
            LsToDisplay = await _context.LearningSets
                .Include(ls => ls.WordMeaningPairs)
                .FirstOrDefaultAsync(ls => ls.Id == LsId);

            // Reorder WordMeaningPair accorging to Order
            if (LsToDisplay != null)
            {
                LsToDisplay.WordMeaningPairs = LsToDisplay.WordMeaningPairs.OrderBy(wmp => wmp.Order).ToList();
            }
            else
            {
                RedirectToPage("List");
            }

            // Prepare CommentWithUsernames for Razor Page to show username
            var comments = await _context.Comments.ToListAsync();

            // Fetch usernames and map them
            CommentWithUsernames = comments.Select(c => new CommentWithUsername
            {
                Body = c.Body,
                Username = _context.Users.Where(u => u.Id == c.UserId).Select(u => u.UserName)
                             .FirstOrDefault() ?? "Unknown"
            }).ToList();

        }

        public class CommentWithUsername()
        {
            public string Body { get; set; }
            public string Username { get; set; }
        }


        
    }
}
