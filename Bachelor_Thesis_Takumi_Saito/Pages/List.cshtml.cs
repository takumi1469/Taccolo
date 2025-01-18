using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bachelor_Thesis_Takumi_Saito.Pages.Data;
using Microsoft.EntityFrameworkCore;

namespace Bachelor_Thesis_Takumi_Saito.Pages
{
    public class ListModel : PageModel
    {
        private readonly Bachelor_Thesis_Takumi_Saito.Pages.Data.AppDbContext _context;

        public ListModel(Bachelor_Thesis_Takumi_Saito.Pages.Data.AppDbContext context)
        {
            _context = context;
        }

        public List<LearningSet> LearningSets { get; set; } 

        public async Task OnGetAsync()
        {
            LearningSets = await _context.LearningSets.ToListAsync(); // Fetching data from the database
        }


        [BindProperty]
        public LearningSet LearningSet { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.LearningSets.Add(LearningSet);
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
