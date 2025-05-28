using Taccolo.Pages.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Security.Claims;
using static Taccolo.Pages.EditViewLsModel;
using Taccolo.Controllers;
using Taccolo.Pages.Shared;

namespace Taccolo.Pages
{
    public class EditViewLsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        [BindProperty(SupportsGet = true, Name = "lsid")] // This binds the route parameter to LsId
        public Guid? LsId { get; set; }
        public string Username { get; set; }
        public LearningSet? LsToDisplay { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<CommentWithUsernameSlug> CommentWithUsernameSlugs { get; set; } = new List<CommentWithUsernameSlug>();
        public List<HelpReplyWithUsernameSlug> HelpReplyWithUsernameSlugs { get; set; } = new List<HelpReplyWithUsernameSlug>();
        public List<HelpRequest> CurrentHelpRequests { get; set; }

        public bool IsOwner { get; set; } = false;
        public bool IsFavorite { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;

        public EditViewLsModel(UserManager<ApplicationUser> userManager,
            AppDbContext context,
            ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("***Logging Test 1***");
            _logger.LogInformation("LsId is: " + LsId.ToString());

            if (LsId == null || LsId == Guid.Empty )
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
                if (user != null)
                    IsAuthenticated = true;

                if (LsToDisplay?.UserId != null)
                    Username = _context.Users.FirstOrDefault(us => us.Id == LsToDisplay.UserId)?.UserName;

                _logger.LogInformation("***Logging Test 2: Before populating IsOwner*** -> passed");
                if (user != null)
                {
                    if(user.Id == LsToDisplay.UserId)
                    {
                        IsOwner = true; // otherwise default is false
                        _logger.LogInformation("***Logging Test 3: After populating IsOwner******");
                    }

                    if (_context.FavoriteSets.Any(fav => fav.UserId == user.Id && fav.LsId == LsId))
                    {
                        _logger.LogInformation("***Logging Test 4: Before determining IsFavorite***");
                        IsFavorite = true;
                        _logger.LogInformation("***Logging Test 5: After determining IsFavorite***");
                    }
                }

                // Reorder WordMeaningPair accorging to Order
                    LsToDisplay.WordMeaningPairs = LsToDisplay.WordMeaningPairs.OrderBy(wmp => wmp.Order).ToList();
                
                // Prepare CommentWithUsernames for Razor Page to show username
                Comments = await _context.Comments.Where(c => c.LsId == LsId).ToListAsync();

                _logger.LogInformation("***Logging Test 6: Before making CommentWithUsernames***");

                // Fetch usernames and map them
                CommentWithUsernameSlugs = Comments.Select(c => new CommentWithUsernameSlug
                {
                    Body = c.Body,
                    Username = _context.Users.Where(u => u.Id == c.UserId).Select(u => u.UserName)
                                 .FirstOrDefault() ?? "Unknown",
                    Slug = _context.Users.Where(u => u.Id == c.UserId).Select(u => u.PublicSlug)
                                 .FirstOrDefault() ?? "Unknown",
                    Date = c.Date,
                    
                }).ToList();

                // Now prepare HelpRequests
                CurrentHelpRequests = await _context.HelpRequests.Where(hr => hr.LsId == LsId).ToListAsync();
                var helpReplys = new List<HelpReply>();
                foreach (HelpRequest helpRequest in CurrentHelpRequests)
                {
                    var newReplys = await _context.HelpReplys.Where(helpReply => helpReply.RequestId == helpRequest.Id).ToListAsync();
                    helpReplys.AddRange(newReplys);
                }

                HelpReplyWithUsernameSlugs = helpReplys.Select(r => new HelpReplyWithUsernameSlug
                {
                    Body = r.Body,
                    Username = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.UserName)
                                 .FirstOrDefault() ?? "Unknown",
                    Slug = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.PublicSlug)
                                 .FirstOrDefault() ?? "Unknown",
                    Date = r.Date,
                    RequestId = r.RequestId
                }).ToList();

                return Page();
            }
        }

        public async Task<IActionResult> OnPostFlashcard()
        {
            LsToDisplay = await _context.LearningSets
               .FirstOrDefaultAsync(ls => ls.Id == LsId);

            _logger.LogInformation($"***LsId is {LsId}***");

            if (LsToDisplay is not null)
                return RedirectToPage("Flashcard", new { lsid = LsToDisplay.Id });
            else
                return RedirectToPage("Error");
        }

        public async Task<IActionResult> OnPostUserPage()
        {
            LsToDisplay = await _context.LearningSets
                 .FirstOrDefaultAsync(ls => ls.Id == LsId);

            _logger.LogInformation($"***LsId is {LsId}***");

            ApplicationUser? user = _context.Users.FirstOrDefault(u => u.Id == LsToDisplay.UserId);
            string slugToPass = user?.PublicSlug;

            _logger.LogInformation($"***slugToPass is {slugToPass}***");

            if (slugToPass is not null)
                return RedirectToPage("UserPage", new { slug = slugToPass });
            else
                return RedirectToPage("Error");

        }

        public class CommentWithUsernameSlug()
        {
            public string Body { get; set; }
            public string Username { get; set; }
            public string? Date { get; set; }
            public string Slug {  get; set; }
        }

        public class HelpReplyWithUsernameSlug()
        {
            public string Body { get; set; }
            public string Username { get; set; }
            public string? Date { get; set; }
            public Guid RequestId { get; set; }
            public string Slug { get; set; }
        }
    }
}
