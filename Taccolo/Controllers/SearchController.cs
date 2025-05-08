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
        public string? MatchAndOr { get; set; }

        public SearchController(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("SearchLsTop")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult SearchLsTop([FromBody] SearchQueryDto searchQuery)
        {
            _logger.LogInformation("***SearchLs Endpoint triggered***");

            // First populate the properties from Dto.
            // Keywords from JavaScript is just one string split it into words and put words in list
            if(searchQuery.Keywords is null || searchQuery.Keywords == string.Empty)
            {
                Keywords = null;
            }
            else
            {
                Keywords = searchQuery.Keywords.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            SourceLanguage = searchQuery.SourceLanguage;
            TargetLanguage = searchQuery.TargetLanguage;
            MatchAndOr = searchQuery.MatchAndOr;


            // Second, search LearningSets by keyword matching
            // Use anonymous object because including User and returning JSON of LS and User directly
            // causes circular reference
            var allSets = _context.LearningSets.Include(ls => ls.User).Select(ls => new
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

            if (Keywords is not null)
            {
                var keywordSets = allSets.Where(ls => Keywords.Any(k =>
                    (ls.Title?.Contains(k) ?? false) ||
                    (ls.Input?.Contains(k) ?? false) ||
                    (ls.Translation?.Contains(k) ?? false) ||
                    (ls.Description?.Contains(k) ?? false) ||
                    (ls.UserName?.Contains(k) ?? false)))
               .ToList();

                // Rank LearningSets according to match counts
                var rankedSets = keywordSets.Select(ls => new {
                    LearningSetAnonymous = ls,
                    MatchCount = Keywords.Count(k =>
                     (ls.Title?.Contains(k) ?? false) ||
                     (ls.Description?.Contains(k) ?? false) ||
                     (ls.Input?.Contains(k) ?? false) ||
                     (ls.Translation?.Contains(k) ?? false) ||
                     (ls.UserName?.Contains(k) ?? false))
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
                    var sourceAndTargetSets = allSets.Where(ls => ls.SourceLanguage ==  SourceLanguage && ls.TargetLanguage == TargetLanguage).ToList();
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

            return Ok(allSets);
        }
    }
}
