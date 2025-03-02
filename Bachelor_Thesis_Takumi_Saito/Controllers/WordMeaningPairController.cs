using Microsoft.AspNetCore.Mvc;
using static Bachelor_Thesis_Takumi_Saito.Pages.EditViewLsModel;
using Bachelor_Thesis_Takumi_Saito.Dtos;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Bachelor_Thesis_Takumi_Saito.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace Bachelor_Thesis_Takumi_Saito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordMeaningPairController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public WordMeaningPairController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("UpdateWmp")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult UpdateWordMeaningPairs([FromBody] UpdateWmpsDto updatedData)
        {
            List<WordMeaningPair> tempWMPs = updatedData.WordMeaningPairs.Select(dto => new WordMeaningPair
            {
                Id = dto.Id,
                LsId = dto.LsId,
                Word = dto.Word,
                TranslatedText = dto.TranslatedText,
                Alternatives = dto.Alternatives,
                Order = dto.Order
            }).ToList();

            // Extract LsId from the first WMP to narrow down the search
            var currentLsId = tempWMPs.First().LsId;

            foreach (var wmp in tempWMPs)
            {
                if (wmp.Id == Guid.Empty)
                {
                    // New WMP, add to context
                    var newWmp = new WordMeaningPair
                    {
                        Id = Guid.NewGuid(),
                        LsId = wmp.LsId,
                        Word = wmp.Word,
                        TranslatedText = wmp.TranslatedText,
                        Alternatives = wmp.Alternatives,
                        Order = wmp.Order
                    };
                    _context.WordMeaningPairs.Add(newWmp);
                }
                else
                {
                    // Existing WMP, update it

                    //var existingWmp = currentLs.WordMeaningPairs.FirstOrDefault(x => x.Id == wmp.Id);

                    var existingWmp = _context.WordMeaningPairs
                           .Where(w => w.LsId == currentLsId && w.Id == wmp.Id)
                           .FirstOrDefault();
                    if (existingWmp != null)
                    {
                        existingWmp.Word = wmp.Word;
                        existingWmp.TranslatedText = wmp.TranslatedText;
                        existingWmp.Alternatives = wmp.Alternatives;
                        existingWmp.Order = wmp.Order;
                    }
                }
            }

            if (updatedData.WmpsToDelete != null)
            {
                foreach (var guid in updatedData.WmpsToDelete)
                {
                    //var wmpToDelete = _context.WordMeaningPairs.Find(guid);
                    var wmpToDelete = _context.WordMeaningPairs
                        .Where(w => w.LsId == currentLsId && w.Id == guid)
                        .FirstOrDefault();
                        
                    if (wmpToDelete != null)
                    {
                        _context.WordMeaningPairs.Remove(wmpToDelete);
                    }
                }
            }

            // Save changes to the database, after all the changes are made
            _context.SaveChanges();

            return new JsonResult(new { success = true, message = "WMPs updated successfully" });
        }
    }
}
