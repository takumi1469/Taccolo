using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class EditViewLSModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _context;

        public LearningSet CurrentLearningSet { get; set; }

        public void OnGet()
        {
        }
    }
}
