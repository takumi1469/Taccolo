using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taccolo.Pages.Data;

namespace Taccolo.Pages
{
    public class OwnLSModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<EditViewLsModel> _logger;

        public List<LearningSet> OwnLearningSets {get; set;} = new List<LearningSet>();

        public OwnLSModel(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<EditViewLsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User); //Gets user information from DI
            if(user == null) { }
            else
            {
                OwnLearningSets = _context.LearningSets.Where(ls => ls.UserId == user.Id).ToList();
                OwnLearningSets.Reverse();
            } 
            
            
        }


    }
}
