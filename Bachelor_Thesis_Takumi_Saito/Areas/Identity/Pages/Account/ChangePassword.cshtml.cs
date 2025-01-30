using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bachelor_Thesis_Takumi_Saito.Areas.Identity.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public InputModel Input { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var user = _userManager.GetUserAsync(User);
        }


        public class InputModel
        {
            [Required]
            public string CurrentPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "NewPassword")]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "ConfirmNewPassword")]
            [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmNewPassword { get; set; }
        }
    }
}
