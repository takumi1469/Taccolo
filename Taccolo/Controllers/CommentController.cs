using Taccolo.Pages.Data;
using Taccolo.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taccolo.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Taccolo.Controllers
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
        public async Task<IActionResult> AddComment([FromBody] CommentDto updatedData)
        {
            _logger.LogInformation("***AddComment Endpoint triggered***");
            
            string userId = _userManager.GetUserId(User);
            var currentUser = await _userManager.GetUserAsync(User);
            string userSlug = "";
            if (currentUser != null)
            {
                userSlug = currentUser.PublicSlug;
            }

            Comment newComment = new Comment(updatedData.Body, updatedData.LsId, userId, updatedData.Date);

            _context.Comments.Add(newComment);

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "LearningSet updated successfully", slug = userSlug});
        }
    }
}
