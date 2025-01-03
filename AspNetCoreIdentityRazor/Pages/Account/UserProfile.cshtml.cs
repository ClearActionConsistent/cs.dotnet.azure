using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentityRazor.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        public UserManager<CustomUser> userManager { get; }

        [BindProperty]
        public UserProfileViewModel userProfileViewModel { get; set; } = new UserProfileViewModel();

        public UserProfileModel(UserManager<CustomUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGetAsync()
        {
            //get logged in user email & claims 
            var positionClaim = User?.Claims.FirstOrDefault(x => x.Type == "position");
            var roleClaim = User?.Claims.FirstOrDefault(x => x.Type == "role");

            userProfileViewModel.Email = User?.Identity?.Name ?? "";
            userProfileViewModel.Role = roleClaim?.Value ?? "";
            userProfileViewModel.Position = positionClaim?.Value ?? "";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(this.userProfileViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("User", "Update userprofile failed.");
                return Page();
            }
            
            var positionClaim = User?.Claims.FirstOrDefault(x => x.Type == "position");
            if (positionClaim != null)
            {
                var x = await this.userManager.ReplaceClaimAsync(
                    user!,
                    positionClaim!,
                    new Claim(positionClaim!.Type, this.userProfileViewModel.Position)
                );
            }
                
            var roleClaim = User?.Claims.FirstOrDefault(x => x.Type == "role");
            if (roleClaim != null)
            {
                var x = await this.userManager.ReplaceClaimAsync(
                    user!,
                    roleClaim!,
                    new Claim(roleClaim!.Type, this.userProfileViewModel.Role)
                );
            }

            return Page();
        }
    }

    public class UserProfileViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;
    }
}
