using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Taccolo.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        [BindProperty]
        public string? SearchKeyword { get; set; }

        public void OnPostSearch()
        {
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                Console.WriteLine("***OnPostSearch is called***");
                RedirectToPage("Index", new { keyword = SearchKeyword });
            }
        }
    }
}
