using Microsoft.AspNetCore.Mvc;
using static Taccolo.Pages.EditViewLsModel;
using Taccolo.Dtos;
using Taccolo.Pages.Data;
using Taccolo.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace Taccolo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LearningSetController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public LearningSetController(UserManager<ApplicationUser> userManager,
            AppDbContext context,
            ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("UpdateLs")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult UpdateLearningSet([FromBody] UpdateLsDto updatedData)
        {
            _logger.LogInformation("***UpdateLs Endpoint triggered***");

            var currentLs = _context.LearningSets
        .Include(ls => ls.WordMeaningPairs)
        .FirstOrDefault(ls => ls.Id == updatedData.Id);

            if (currentLs == null)
            {
                return new JsonResult(new { success = false, message = "LearningSet not found" });
            }

            // Update original text and translation of current LS based on the incoming data
            currentLs.Input = updatedData.OriginalText;
            currentLs.Translation = updatedData.TranslatedText;
 
            // Save changes to the database, after all the changes are made
            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully" });
        }

        [HttpPost("UpdateLsDescription")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult UpdateLsDescription([FromBody] UpdateDescriptionDto updatedData)
        {
            _logger.LogInformation("***UpdateLsDescription Endpoint triggered***");

            var currentLs = _context.LearningSets
        .Include(ls => ls.WordMeaningPairs)
        .FirstOrDefault(ls => ls.Id == updatedData.LsId);

            if (currentLs == null)
            {
                return new JsonResult(new { success = false, message = "LearningSet not found" });
            }

            // Update description
            currentLs.Description = updatedData.Description;

            // Save changes to the database, after all the changes are made
            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "Description updated successfully" });
        }
    }
}
