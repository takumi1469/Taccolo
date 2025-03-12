using Bachelor_Thesis_Takumi_Saito.Dtos;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Bachelor_Thesis_Takumi_Saito.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bachelor_Thesis_Takumi_Saito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpReplyController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public HelpReplyController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddHelpReply")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult AddHelpReply([FromBody] HelpReplyDto updatedData)
        {
            _logger.LogInformation("***AddHelpRequest Endpoint triggered***");
            string? userId = _userManager.GetUserId(User);

            HelpReply newHelpReply = new HelpReply(updatedData.Body, userId, updatedData.RequestId, updatedData.Date);

            _context.HelpReplys.Add(newHelpReply);

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully" });

        }
    }
}
