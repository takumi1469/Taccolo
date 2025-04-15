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
    public class LsToShowController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        //public List<LearningSet> AllLearningSets { get; set; } = new List<LearningSet>();

        public LsToShowController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet("PassAllLs")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult PassAllLs()
        {
            _logger.LogInformation("***PassAllLs Endpoint triggered***");

            var allLearningSets = _context.LearningSets
                .Include(ls => ls.User)
                .Select(ls => new
                {
                    Id = ls.Id,
                    Username = ls.User.UserName,
                    Date = ls.Date,
                    SourceLanguage = ls.SourceLanguage,
                    TargetLanguage = ls.TargetLanguage,
                    Input = ls.Input,
                })
                .ToList();

            return Ok(allLearningSets);
        }
    }
}