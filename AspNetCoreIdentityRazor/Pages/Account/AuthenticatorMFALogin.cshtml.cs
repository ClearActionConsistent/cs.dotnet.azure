using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class AuthenticatorMFALoginModel : PageModel
    {
        private readonly SignInManager<CustomUser> signInManager;

        [BindProperty]
        public AuthenticatorMFA authenticatorMFA { get; set; }
        public readonly UserManager<CustomUser> UserManager;
        
        public AuthenticatorMFALoginModel(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            UserManager = userManager;
            this.signInManager = signInManager;
            authenticatorMFA = new AuthenticatorMFA();
        }

        public void OnGet(bool rememberMe)
        {
            this.authenticatorMFA.RememberMe = rememberMe;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            //a cookie was generated when user provide email/pass before redirecting to this page
            //and the cookie was sent when postback, that cookie let SignInManager know which use to logged in
            var result = await this.signInManager.TwoFactorAuthenticatorSignInAsync(this.authenticatorMFA.Code,
                this.authenticatorMFA.RememberMe, false);
            
            if(!result.Succeeded)
            {
                ModelState.AddModelError("AuthenticatorMFALogin", "Two factor authentication failed.");
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

    public class AuthenticatorMFA
    {
        [Required]
        [Display(Name = "Security Code")]
        public string Code { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
