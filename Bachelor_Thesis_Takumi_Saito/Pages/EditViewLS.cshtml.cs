using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Security.Claims;
using static Bachelor_Thesis_Takumi_Saito.Pages.EditViewLsModel;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class EditViewLsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        [BindProperty(SupportsGet = true, Name = "lsid")] // This binds the route parameter to LsId
        public Guid? LsId { get; set; }
        public LearningSet? LsToDisplay { get; set; }
        public List<CommentWithUsername> CommentWithUsernames { get; set; } = new List<CommentWithUsername>();
        public List<HelpReplyWithUsername> HelpReplyWithUsernames { get; set; } = new List<HelpReplyWithUsername>();
        public List<HelpRequest> CurrentHelpRequests { get; set; }

        public bool IsOwner { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;

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
            if (LsId is null)
                RedirectToPage("Index");
       
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user != null)
                IsAuthenticated = true;

            // Fetch the LearningSet using ID together with associated WordMeaningPair
            LsToDisplay = await _context.LearningSets
                .Include(ls => ls.WordMeaningPairs)
                .FirstOrDefaultAsync(ls => ls.Id == LsId);

            if (user != null && user.Id == LsToDisplay.UserId)
                IsOwner = true; // otherwise default is false

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
            var comments = await _context.Comments.Where(c => c.LsId == LsId).ToListAsync();

            // Fetch usernames and map them
            CommentWithUsernames = comments.Select(c => new CommentWithUsername
            {
                Body = c.Body,
                Username = _context.Users.Where(u => u.Id == c.UserId).Select(u => u.UserName)
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

            HelpReplyWithUsernames = helpReplys.Select(r => new HelpReplyWithUsername
            {
                Body = r.Body,
                Username = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.UserName)
                             .FirstOrDefault() ?? "Unknown",
                Date = r.Date,
                RequestId = r.RequestId
            }).ToList();

        }



        public class CommentWithUsername()
        {
            public string Body { get; set; }
            public string Username { get; set; }
            public string? Date { get; set; }
        }

        public class HelpReplyWithUsername()
        {
            public string Body { get; set; }
            public string Username { get; set; }
            public string? Date { get; set; }
            public Guid RequestId { get; set; }
        }


        
    }
}
