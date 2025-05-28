using Taccolo.Dtos;
using Taccolo.Pages.Data;
using Taccolo.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Taccolo.Controllers
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
        public async Task<IActionResult> AddHelpReply([FromBody] HelpReplyDto updatedData)
        {
            _logger.LogInformation("***AddHelpRequest Endpoint triggered***");
            string? userId = _userManager.GetUserId(User);
            var currentUser = await _userManager.GetUserAsync(User);
            string userSlug = "";
            if (currentUser != null)
            {
                userSlug = currentUser.PublicSlug;
            }

            HelpReply newHelpReply = new HelpReply(updatedData.Body, userId, updatedData.RequestId, updatedData.Date);

            _context.HelpReplys.Add(newHelpReply);

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully", slug = userSlug });

        }
    }
}
