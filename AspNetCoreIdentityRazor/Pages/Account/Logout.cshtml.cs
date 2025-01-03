using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public SignInManager<IdentityUser> SignInManager { get; }
        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            await this.SignInManager.SignOutAsync();
            RedirectToPage("/Account/Login");
        }
    }
}
