using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class RegistrationCompleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegistrationCompleteModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegistrationCompleteModel(UserManager<ApplicationUser> userManager, ILogger<RegistrationCompleteModel> logger, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager; 
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            //The below block is for when email verification is required
            if (userId == null || code == null)
                {
                    return RedirectToPage("/Index");
                }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                IsSuccess = true;
                await _signInManager.SignInAsync(user,true);//NOT TESTED YET!
            }
            else
            {
                ErrorMessage = "Invalid or expired token.";
            }

            return Page();
        }
    }
}
