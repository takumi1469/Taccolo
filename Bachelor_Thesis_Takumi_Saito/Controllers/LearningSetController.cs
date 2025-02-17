using Microsoft.AspNetCore.Mvc;
using static Bachelor_Thesis_Takumi_Saito.Pages.EditViewLsModel;
using Bachelor_Thesis_Takumi_Saito.Dtos;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Bachelor_Thesis_Takumi_Saito.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Bachelor_Thesis_Takumi_Saito.Controllers
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

            var currentLs = _context.LearningSets.Find(updatedData.Id);

            if (currentLs == null)
            {
                return new JsonResult(new { success = false, message = "LearningSet not found" });
            }

            // Update properties of current LS based on the incoming data
            currentLs.Input = updatedData.OriginalText;
            currentLs.Translation = updatedData.TranslatedText;
            currentLs.WordMeaningPairs = updatedData.WordMeaningPairs?.Select(dto => new WordMeaningPair
            {
                Id = dto.Id,
                LsId = dto.LsId,
                Word = dto.Word,
                TranslatedText = dto.TranslatedText,
                Alternatives = dto.Alternatives,
                Order = dto.Order
            }).ToList();






            // Save changes to the database
            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully TEST" });
        }

       

    }
}
