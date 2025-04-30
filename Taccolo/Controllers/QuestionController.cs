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
    public class QuestionController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        //[BindProperty(SupportsGet = true, Name = "lsid")] //[BindProperty] is for query parameter and form elements
        [FromRoute]
        public Guid LsId { get; set; }
        public List<FlashcardQuestion> Questions { get; set; } = new List<FlashcardQuestion>();
        public List<string> WordPool { get; set; } = new List<string>();
        public LearningSet CurrentLs { get; set; }
        public List<WordMeaningPair>? CurrentWmp { get; set; } = new List<WordMeaningPair>();
        static Random random = new Random();

        public QuestionController(UserManager<ApplicationUser> userManager,
                AppDbContext context,
                ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public void LoadData()
        {
            CurrentLs = _context.LearningSets
            .Include(ls => ls.WordMeaningPairs)
            .FirstOrDefault(ls => ls.Id == LsId);
            if (CurrentLs == null)
                _logger.LogWarning("LearningSet not found for LsId: {LsId}", LsId);

            CurrentWmp = CurrentLs?.WordMeaningPairs;
            if (CurrentWmp == null)
                _logger.LogWarning("CurrentWmp is null");
        }
        public void MakeWordPool()
        {                   
            if(CurrentWmp is null) {}
            else
            {
                foreach (WordMeaningPair wmp in CurrentWmp)
                {
                    if (wmp.TranslatedText is null) { }
                    else
                    {
                        WordPool.Add(wmp.TranslatedText);
                    }
                    if(wmp.Alternatives is null) { }
                    else
                    {
                        foreach (string alternative in wmp.Alternatives)
                        {
                            WordPool.Add(alternative);
                        }
                    }
                }
            }
        }

        public void MakeQuestions()
        {
            foreach(WordMeaningPair wmp in CurrentWmp)
            {
                FlashcardQuestion NewQuestion = new FlashcardQuestion();
                NewQuestion.Word = wmp.Word;
                NewQuestion.CorrectChoice = wmp.TranslatedText;

                for(int i = 0;i<3;i++)
                {
                    int randomIndex = random.Next(0, WordPool.Count);
                    if (WordPool[randomIndex] == NewQuestion.CorrectChoice) { }
                    else
                    {
                        NewQuestion.WrongChoices.Add(WordPool[randomIndex]);
                    }
                }

                Questions.Add(NewQuestion);
            }
        }

        [HttpGet("GiveQuestions/{LsId}")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public IActionResult GiveQuestions()
        {
            _logger.LogInformation("***GiveQuestions Endpoint triggered***");
            LoadData();
            MakeWordPool();
            MakeQuestions();
            return Ok(Questions);
        }
    }
}
