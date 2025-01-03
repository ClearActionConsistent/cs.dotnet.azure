using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class LoginModel : PageModel
    {
        public SignInManager<CustomUser> SignInManager { get; }
        public LoginModel(SignInManager<CustomUser> userManager)
        {
            SignInManager = userManager;
        }
        [BindProperty]
        public CredentialViewModel Credential { get; set; } = new CredentialViewModel();
        

        public void OnGet()
        {
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
                return RedirectToPage("Index");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "You are locked out.");
            }
            else
            {
                ModelState.AddModelError("Login", "Failed to login.");
            }
            return Page();
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
