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
    public class RegisterSlugController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public RegisterSlugController(UserManager<ApplicationUser> userManager,
               AppDbContext context,
               ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("CheckSlug")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CheckSlug([FromBody] SlugDto updatedData)
        {
            _logger.LogInformation("***SlugCheck Endpoint triggered***");

            string slugToCheck = updatedData.Slug;
            bool checkResult = await _context.Users.AnyAsync(user => user.PublicSlug == slugToCheck);
            return Ok(checkResult);
        }
    }
}
