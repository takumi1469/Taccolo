using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class MyPageEditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<MyPageEditModel> _logger;
        private readonly IEmailSender _emailSender;

        public MyPageEditModel(UserManager<ApplicationUser> userManager,
            ILogger<MyPageEditModel> logger,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public ApplicationUser? CurrentUser { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            Input = new InputModel();//Need to explicitly instantiate. OnPost automatically instantiates Input when binding form to property
            Input.Bio = CurrentUser?.Bio ?? "Default bio text here"; // Without this, textarea for Bio isn't prepopulated
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid && CurrentUser != null)
            {
                string returnUrl = Url.Content("~/");

                CurrentUser.UserName = Input.UserName; 
                CurrentUser.Email = Input.Email; 
                CurrentUser.DesiredLanguages = Input.DesiredLanguages;
                CurrentUser.KnownLanguages = Input.KnownLanguages;
                CurrentUser.Bio = Input.Bio;

                // If the user is changing email, verification is required
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    if (Input.Email != CurrentUser.Email)
                    {
                        var userId = await _userManager.GetUserIdAsync(CurrentUser);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(CurrentUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/RegistrationComplete",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    }
                }

                // Save changes to the user
                var updateResult = await _userManager.UpdateAsync(CurrentUser);

                if (updateResult.Succeeded)
                {
                    // Redirect or show a success message
                    await _signInManager.RefreshSignInAsync(CurrentUser); //This is for updating authentication cookie, where @User.Identity.Name comes from
                    return RedirectToPage("/List");
                }
                else
                {
                    // Handle any errors during the update
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return RedirectToPage("/Words");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToPage("/Privacy");
        }




        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public List<string>? DesiredLanguages { get; set; }
            public List<string>? KnownLanguages { get; set; }

            public string? Bio {  get; set; }

        }
    }
}
