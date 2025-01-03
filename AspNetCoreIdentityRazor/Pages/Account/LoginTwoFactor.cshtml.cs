using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class LoginTwoFactorModel : PageModel
    {
        private readonly SignInManager<CustomUser> signInManager;

        [BindProperty]
        public EmailMFA EmailMFA { get; set; }
        public readonly UserManager<CustomUser> UserManager;
        
        public LoginTwoFactorModel(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            UserManager = userManager;
            this.signInManager = signInManager;
            EmailMFA = new EmailMFA();
        }

        public async Task<IActionResult> OnGet(string email, bool rememberMe)
        {
            this.EmailMFA.RememberMe = rememberMe;

            var user = await this.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("Login", "User does not exist.");
                return Page();
            }

            var OTPCode = await this.UserManager.GenerateTwoFactorTokenAsync(user, "Email");

            //send this token to the email
            //for demo purpose, just log the code to console
            Console.WriteLine(OTPCode);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            var result = await this.signInManager.TwoFactorSignInAsync("Email", this.EmailMFA.Code, this.EmailMFA.RememberMe, false);
            
            if(!result.Succeeded)
            {
                ModelState.AddModelError("TwoFactor", "Two factor authentication failed.");
                return Page();
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "You are locked out.");
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }

    public class EmailMFA
    {
        [Required]
        [Display(Name = "Security Code")]
        public string Code { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
