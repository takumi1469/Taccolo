using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Bachelor_Thesis_Takumi_Saito.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bachelor_Thesis_Takumi_Saito.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_Thesis_Takumi_Saito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public CommentController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddComment")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult AddComment([FromBody] CommentDto updatedData)
        {
            _logger.LogInformation("***AddComment Endpoint triggered***");
            
            string userId = _userManager.GetUserId(User);
            Comment newComment = new Comment(updatedData.Body, updatedData.LsId, userId, "dummyDate");

            _context.Comments.Add(newComment);

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully" });


        }


    }
}
