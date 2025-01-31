using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ForgotPasswordModel> _logger;
        private readonly IEmailSender _emailSender;

        public string SentMessage {  get; set; } = string.Empty;
        public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
            ILogger<ForgotPasswordModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if(user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ForgotPasswordReset",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, email=Input.Email},
                    protocol: Request.Scheme);
                Console.WriteLine($"Encoded token: {code}");

                await _emailSender.SendEmailAsync(Input.Email,"Reset password for taccolo",
                    $"You can reset your password from <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a>.");
            }

            SentMessage = "Email has been sent to the specified email for password reset.</br>" +
                "You can go to home from <a class=\"a-explicit-link\" href=\"/\">here</a>";

            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email {  get; set; }
        }

    }
}
