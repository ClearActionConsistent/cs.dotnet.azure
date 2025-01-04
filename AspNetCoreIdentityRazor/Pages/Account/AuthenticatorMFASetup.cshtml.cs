using System.Reflection.Metadata.Ecma335;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    [Authorize]
    public class AuthenticatorMFASetupModel : PageModel
    {
        [BindProperty]
        public AuthenticatorMFAViewModel authenticatorMFAViewModel { get; set; }

        private readonly UserManager<CustomUser> userManager;

        public AuthenticatorMFASetupModel(UserManager<CustomUser> userManager)
        {
            this.userManager = userManager;
            this.authenticatorMFAViewModel = new AuthenticatorMFAViewModel();
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(base.User);
            if (user == null)
            {
                ModelState.AddModelError("AuthenticatorSetup", "Something went wrong, please try again later.");
                return Page();
            }

            //this line will create new row in 
            await this.userManager.ResetAuthenticatorKeyAsync(user);
            var key = await this.userManager.GetAuthenticatorKeyAsync(user!);
            if (string.IsNullOrEmpty(key))
            {
                ModelState.AddModelError("AuthenticatorSetup", "Something went wrong, please try again later.");
                return Page();
            }

            this.authenticatorMFAViewModel.Key = key;



            return Page();
        }

        public void OnPost() 
        { 
            //verify the generated security code generated from Authenticate app
            //log user in
            //enable two factor authentication
            //redirect to homepage
        }

        //private byte[] GenerateQRCodeByes(string provider, string key, string email)
        //{

        //    return null;
        //}
    }

    public class AuthenticatorMFAViewModel
    {
        public string Key { get; set; } = string.Empty;
    }
}
