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

        public SearchController(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        private List<LearningSet> Search(Parameters parameters, string? userId = "no-user", bool? favorite = false)
        {
            _logger.LogInformation("***SearchLs Endpoint triggered***");

            // First turn the keywords into List<string>.
            // Keywords from JavaScript is just one string.
            List<string> Keywords = new List<string>();
            if (parameters.Keywords is null || parameters.Keywords == string.Empty)
            {
                Keywords = null;
            }
            else
            {
                Keywords = parameters.Keywords.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                Keywords = Keywords.Select(k => k.ToLower()).ToList(); //normalize Keywords to lower case
            }

            string SourceLanguage = parameters.SourceLanguage;
            string TargetLanguage = parameters.TargetLanguage;
            string MatchAndOr = parameters.MatchAndOr;

            // Second, search LearningSets by keyword matching
            var allSets = _context.LearningSets.Include(ls => ls.User).ToList();
            if (userId != "no-user" && favorite == false)
            {
                _logger.LogInformation($"***userId is not null, it is {userId}***");
                allSets = allSets.Where(ls => ls.UserId == userId).ToList();
                _logger.LogInformation("*** Length of originalSets is " + allSets.Count().ToString() + "***");
            }

            if(userId != "no-user" && favorite == true)
            {
                allSets = _context.FavoriteSets
                    .Where(fs => fs.UserId == userId)
                    .Include(fs => fs.LearningSet)
                    .Select(fs => fs.LearningSet)
                    .ToList();
            }

            allSets.Reverse();

            if (Keywords is not null)
            {
                var keywordSets = allSets.Where(ls => Keywords.Any(k =>
                    (ls.Title?.ToLower().Contains(k) ?? false) ||
                    (ls.Input?.ToLower().Contains(k) ?? false) ||
                    (ls.Translation?.ToLower().Contains(k) ?? false) ||
                    (ls.Description?.ToLower().Contains(k) ?? false) ||
                    (ls.User?.UserName?.ToLower().Contains(k) ?? false)))
               .ToList();

                // Rank LearningSets according to match counts
                var rankedSets = keywordSets.Select(ls => new
                {
                    LearningSetAnonymous = ls,
                    MatchCount = Keywords.Count(k =>
                     (ls.Title?.Contains(k) ?? false) ||
                     (ls.Description?.Contains(k) ?? false) ||
                     (ls.Input?.Contains(k) ?? false) ||
                     (ls.Translation?.Contains(k) ?? false) ||
                     (ls.User?.UserName?.Contains(k) ?? false))
                })
               .OrderByDescending(x => x.MatchCount)
               .Select(x => x.LearningSetAnonymous)
               .ToList();

                allSets = rankedSets;
            }

            // Third, check if the Source Language and Target Language are both used for search or only one or none
            // Both used
            if (SourceLanguage != "not-chosen" && TargetLanguage != "not-chosen")
            {
                if (MatchAndOr == "AND")
                {
                    _logger.LogInformation("***CASE 1***");
                    var sourceAndTargetSets = allSets.Where(ls => ls.SourceLanguage == SourceLanguage && ls.TargetLanguage == TargetLanguage).ToList();
                    allSets = sourceAndTargetSets;
                }
                else //OR
                {
                    _logger.LogInformation("***CASE 2***");
                    var sourceOrTargetSets = allSets.Where(ls => ls.SourceLanguage == SourceLanguage || ls.TargetLanguage == TargetLanguage).ToList();
                    allSets = sourceOrTargetSets;
                }
            }
            //only source is used
            else if (SourceLanguage != "not-chosen" && TargetLanguage == "not-chosen")
            {
                _logger.LogInformation("***CASE3***");
                var sourceSets = allSets.Where(ls => ls.SourceLanguage == SourceLanguage).ToList();
                allSets = sourceSets;
            }
            //only target is used
            else if (SourceLanguage == "not-chosen" && TargetLanguage != "not-chosen")
            {
                _logger.LogInformation("***CASE 4***");
                var targetSets = allSets.Where(ls => ls.TargetLanguage == TargetLanguage).ToList();
                allSets = targetSets;
            }
            // or else none is used, in that case nothing needs to be done

            return allSets;
        }

        [HttpPost("SearchLsTop")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult SearchLsTop([FromBody] SearchQueryDto searchQuery)
        {
            Parameters parameters = new Parameters
            (
                keywords: searchQuery.Keywords,
                sourceLanguage: searchQuery.SourceLanguage,
                targetLanguage: searchQuery.TargetLanguage,
                matchAndOr: searchQuery.MatchAndOr
            );

            var MatchingSets = Search(parameters).Select(ls => new
            {
                ls.Title,
                ls.Id,
                ls.Input,
                ls.Translation,
                ls.SourceLanguage,
                ls.TargetLanguage,
                UserName = ls.User.UserName,
                ls.Date,
                ls.Description
            }).ToList();

           return Ok(MatchingSets);
        }


        [HttpPost("SearchLsOwn")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SearchLsOwn([FromBody] SearchQueryDto searchQuery)
        {
            _logger.LogInformation("***SearchLsOwn has been called***");

            ApplicationUser? user = await _userManager.GetUserAsync(User);
            string userId = "default-id";
            if (user != null)
            {
                userId = user.Id;
                _logger.LogInformation($"***userId = {userId} ***");
            }

            Parameters parameters = new Parameters
           (
               keywords: searchQuery.Keywords,
               sourceLanguage: searchQuery.SourceLanguage,
               targetLanguage: searchQuery.TargetLanguage,
               matchAndOr: searchQuery.MatchAndOr
           );

            var MatchingSets = Search(parameters, userId).Select(ls => new
            {
                ls.Title,
                ls.Id,
                ls.Input,
                ls.Translation,
                ls.SourceLanguage,
                ls.TargetLanguage,
                UserName = ls.User.UserName,
                ls.Date,
                ls.Description
            }).ToList();

            return Ok(MatchingSets);
        }

        [HttpPost("SearchLsFavorite")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SearchLsFavorite([FromBody] SearchQueryDto searchQuery)
        {
            _logger.LogInformation("***SearchLsFavorite has been called***");

            //string userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            string userId = "default-id";
            if (user != null)
            {
                userId = user.Id;
                _logger.LogInformation($"***userId = {userId} ***");
            }

            Parameters parameters = new Parameters
           (
               keywords: searchQuery.Keywords,
               sourceLanguage: searchQuery.SourceLanguage,
               targetLanguage: searchQuery.TargetLanguage,
               matchAndOr: searchQuery.MatchAndOr
           );

            var MatchingSets = Search(parameters, userId, true).Select(ls => new
            {
                ls.Title,
                ls.Id,
                ls.Input,
                ls.Translation,
                ls.SourceLanguage,
                ls.TargetLanguage,
                UserName = ls.User.UserName,
                ls.Date,
                ls.Description
            }).ToList();

            return Ok(MatchingSets);
        }

        [HttpPost("SearchLsUser")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SearchLsUser([FromBody] SearchQueryDto searchQuery)
        {
            _logger.LogInformation("***SearchLsUser has been called***");

            string slug = "";
            string userId = "default-id";
            if (searchQuery.Slug is not null)
            {
                slug = searchQuery.Slug;
            }

            ApplicationUser user = _context.Users.FirstOrDefault(u=> u.PublicSlug == slug);
            if(user is not null)
            {
                userId = user.Id;
                _logger.LogInformation($"***userId = {userId} ***");
            }

            Parameters parameters = new Parameters
           (
               keywords: searchQuery.Keywords,
               sourceLanguage: searchQuery.SourceLanguage,
               targetLanguage: searchQuery.TargetLanguage,
               matchAndOr: searchQuery.MatchAndOr
           );

            var MatchingSets = Search(parameters, userId).Select(ls => new
            {
                ls.Title,
                ls.Id,
                ls.Input,
                ls.Translation,
                ls.SourceLanguage,
                ls.TargetLanguage,
                UserName = ls.User.UserName,
                ls.Date,
                ls.Description
            }).ToList();

            return Ok(MatchingSets);
        }

        public class Parameters
        {
            public string? Keywords { get; set; }
            public string SourceLanguage { get; set; }
            public string TargetLanguage { get; set; }
            public string MatchAndOr { get; set; }

            public Parameters(string keywords, string sourceLanguage, string targetLanguage, string matchAndOr)
            {
                Keywords = keywords;
                SourceLanguage = sourceLanguage;
                TargetLanguage = targetLanguage;
                MatchAndOr = matchAndOr;
            }
        }
    }
}
