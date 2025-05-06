using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taccolo.Pages.Data;
using Taccolo.Pages;
using Microsoft.AspNetCore.Authorization;
using Taccolo.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Taccolo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public List<string>? Keywords { get; set; } = new List<string>();
        public string? SourceLanguage {  get; set; } 
        public string? TargetLanguage { get; set; }
        public string? matchAndOr { get; set; }

        public SearchController(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("SearchLs")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult SearchLs([FromBody] SearchQueryDto searchQuery)
        {
            _logger.LogInformation("***SearchLs Endpoint triggered***");

            // First, search LearningSets by keyword matching
            // Keywords from JavaScript is just one string split it into words and put words in list
            Keywords = searchQuery.Keywords.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            // Search LearningSets containing keywords
            var matchingSets = _context.LearningSets
                .Include(ls => ls.User)
               .Where(ls => Keywords.Any(k =>
                    ls.Title.Contains(k) ||
                    ls.Input.Contains(k) ||
                    ls.Translation.Contains(k) ||
                    ls.User.UserName.Contains(k))) 
               .ToList();

            // Rank LearningSets according to match counts
            var rankedSets = matchingSets
               .Select(ls => new {
                    LearningSet = ls,
                    MatchCount = Keywords.Count(k =>
                     (ls.Title?.Contains(k) ?? false) ||
                     (ls.Description?.Contains(k) ?? false))
               })
               .OrderByDescending(x => x.MatchCount)
               .Select(x => x.LearningSet)
               .ToList();


            return Ok();
        }
    }
}
