using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class LoginModel : PageModel
    {
        public SignInManager<CustomUser> SignInManager { get; }
        public LoginModel(SignInManager<CustomUser> signInManager)
        {
            SignInManager = signInManager;
        }
        [BindProperty]
        public CredentialViewModel Credential { get; set; } = new CredentialViewModel();

        [BindProperty]
        public IEnumerable<AuthenticationScheme>? ExternalLoginProviders { get; set; }

        public async Task OnGet()
        {
            //get social login providers from signin manager
            this.ExternalLoginProviders = await this.SignInManager.GetExternalAuthenticationSchemesAsync();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            var result = await this.SignInManager.PasswordSignInAsync(
                this.Credential.Email,
                this.Credential.Password,
                this.Credential.RememberMe,
                false
            );

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "You are locked out.");
                return Page();
            }

            if (result.RequiresTwoFactor)
            {
                /*For Email MFA*/
                /*
                //this line of code also write a cookie to browser which will be used to verify the OTP sending in email later on
                return RedirectToPage("/Account/LoginTwoFactor", new {Email = this.Credential.Email, RememberMe = this.Credential.RememberMe});
                */

                /*
                 * Authenticator MFA
                    need to enable two factor authen for the user in db 
                 */

                return RedirectToPage("/Account/AuthenticatorMFALogin", new { RememberMe = this.Credential.RememberMe });
            }
            
            ModelState.AddModelError("Login", "Failed to login.");
            return Page();
        }

        public ActionResult OnPostLoginExternal(string provider)
        {
            var properties = this.SignInManager.ConfigureExternalAuthenticationProperties(provider, null);
            properties.RedirectUri = Url.Action("FacebookLoginCallback", "Auth");
            /*The app is configuring FB authentication handler using MS.ASP.NETCORE.Auth.FB, the handler knows the FB api to log user in and then FB return back user info.
             then ASPNETCORE Identity automatically capture user info returned from FB to create local account with appropriated claims when FB call back to the url before.

            and then in the callback action, we can use signin manager to sign user in with email and name
             */

            return Challenge(properties, provider);
        }
    }

    public class CredentialViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
