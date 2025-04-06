using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Taccolo.Pages.Data;
using Microsoft.EntityFrameworkCore;

namespace Taccolo.Pages
{
    public class ListModel : PageModel
    {
        private readonly Taccolo.Pages.Data.AppDbContext _context;

        public ListModel(Taccolo.Pages.Data.AppDbContext context)
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
