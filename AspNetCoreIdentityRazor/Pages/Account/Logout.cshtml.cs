using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public SignInManager<CustomUser> SignInManager { get; }
        public LogoutModel(SignInManager<CustomUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await this.SignInManager.SignOutAsync();
            return RedirectToPage("/index");
        }
    }
}
