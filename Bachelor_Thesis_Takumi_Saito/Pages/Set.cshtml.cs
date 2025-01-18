using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class SetModel : PageModel
    {
        private readonly Bachelor_Thesis_Takumi_Saito.Pages.Data.AppDbContext _context;

        public SetModel(Bachelor_Thesis_Takumi_Saito.Pages.Data.AppDbContext context)
        {
            _context = context;
        }

        public LearningSet LearningSet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningset = await _context.LearningSets.FirstOrDefaultAsync(m => m.Id == id);
            if (learningset == null)
            {
                return NotFound();
            }
            else
            {
                LearningSet = learningset;
            }
            return Page();
        }
    }
}
