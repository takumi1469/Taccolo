using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class ForgotPasswordResetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ForgotPasswordResetModel> _logger;

        [BindProperty]
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? Code { get; set; }
        public ForgotPasswordResetModel(UserManager<ApplicationUser> userManager,
                    ILogger<ForgotPasswordResetModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public bool UserFound { get; set; } = false;

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string? userId, string? code, string? email)
        {
            Email = email;
            UserId = userId;
            Code = code;

            if (UserId is not null && Code is not null && Email is not null)
            {

                ApplicationUser? user = null;
                if (email != null)
                {
                    user = await _userManager.FindByEmailAsync(email);
                }
                if (user != null)
                {
                    UserFound = true;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(string? userId, string? code, string email)
        {
            UserId = userId;
            Code = code;
            Email = email;
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                UserFound = false;
                return Page();
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, decodedCode, Input.NewPassword);

            if (resetResult.Succeeded)
            {
                return RedirectToPage("ChangePasswordComplete");
            }
            else
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "NewPassword")]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "ConfirmNewPassword")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmNewPassword { get; set; }
        }
    }
}
