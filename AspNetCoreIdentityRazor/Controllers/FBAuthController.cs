using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreIdentityRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FBAuthController : ControllerBase
    {
        private readonly SignInManager<CustomUser> signInManager;

        public FBAuthController(SignInManager<CustomUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> FacebookLoginCallback()
        {
            var loginInfo = await this.signInManager.GetExternalLoginInfoAsync();
            if (loginInfo != null)
            {
                var emailClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                var userClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (emailClaim != null && userClaim != null)
                {
                    var user = new CustomUser { Email = emailClaim.Value, UserName = userClaim.Value };
                    await this.signInManager.SignInAsync(user, false);
                }
            }

            return RedirectToPage("/Index");
        }
       
    }
}
