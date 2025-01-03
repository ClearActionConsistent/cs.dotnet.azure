using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        public UserManager<IdentityUser> UserManager { get; }

        [BindProperty] public string Message { get; set; } = string.Empty;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                Message = "Invalid user ID";
                return Page();
            }

            var result = await this.UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                Message = "Email address is successfully confirmed. You can now login.";
               
            }
            return Page();
        }
    }
}
