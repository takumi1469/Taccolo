using Bachelor_Thesis_Takumi_Saito.Dtos;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Bachelor_Thesis_Takumi_Saito.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace Bachelor_Thesis_Takumi_Saito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpRequestController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public HelpRequestController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddHelpRequest")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult AddHelpRequest([FromBody] HelpRequestDto updatedData)
        {
            _logger.LogInformation("***AddHelpRequest Endpoint triggered***");

            HelpRequest newHelpRequest = new HelpRequest(updatedData.Body, updatedData.LsId);
            newHelpRequest.Id = Guid.NewGuid();

            _context.HelpRequests.Add(newHelpRequest);

            _context.SaveChanges();

            //return new JsonResult(new { success = true, message = "LearningSet updated successfully" });
            return new JsonResult(new { success = true, message = "LearningSet updated successfully", requestId = newHelpRequest.Id });

        }
    }
}
