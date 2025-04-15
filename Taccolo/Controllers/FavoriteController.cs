using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taccolo.Dtos;
using Taccolo.Pages.Data;
using Taccolo.Pages;
using System;

namespace Taccolo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public FavoriteController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddFavorite")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult AddFavorite([FromBody] FavoriteDto updatedData)
        {
            _logger.LogInformation("***AddFavorite Endpoint triggered***");

            string userId = _userManager.GetUserId(User);
            FavoriteSet newFavorite = new FavoriteSet(updatedData.LsId, userId);

            _context.FavoriteSets.Add(newFavorite);

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "Favorite added successfully" });
        }

        [HttpPost("RemoveFavorite")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult RemoveFavorite([FromBody] FavoriteDto updatedData)
        {
            _logger.LogInformation("***RemoveFavorite Endpoint triggered***");

            string userId = _userManager.GetUserId(User);
            FavoriteSet receivedSet = new FavoriteSet(updatedData.LsId, userId);

            var setToRemove = _context.FavoriteSets
                .Where(set => set.LsId == receivedSet.LsId && set.UserId == receivedSet.UserId)
                .FirstOrDefault();

            if (setToRemove != null)
            {
                _context.FavoriteSets.Remove(setToRemove);
            }

            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "Favorite removed successfully" });
        }

    }
}
