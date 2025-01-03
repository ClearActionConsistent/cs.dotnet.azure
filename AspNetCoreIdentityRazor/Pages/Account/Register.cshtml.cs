using System.ComponentModel.DataAnnotations;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();
        public UserManager<CustomUser> UserManager { get; }

        public RegisterModel(UserManager<CustomUser> userManager)
        {
            UserManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            var user = new CustomUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
                Position = RegisterViewModel.Position,
                Role = RegisterViewModel.Role
            };

            var result = await this.UserManager.CreateAsync(user, RegisterViewModel.Password);
            if(result.Succeeded)
            {
                var token = await this.UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.PageLink(pageName: "ConfirmEmail", values: new { userId = user.Id, token = token }) ?? "";

                //send the link to use's email, asking for email confirmation. When user click on the link, next step will be processed.

                return Redirect(confirmationLink);
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
            return Page();
        }

    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
