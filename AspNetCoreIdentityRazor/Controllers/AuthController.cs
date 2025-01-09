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
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<CustomUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthController(SignInManager<CustomUser> signInManager,
            UserManager<CustomUser> userManager,
            IConfiguration configuration)
        {
            this.signInManager = signInManager;
            UserManager = userManager;
            this.configuration = configuration;
        }

        public UserManager<CustomUser> UserManager { get; }

        [HttpPost]
        public async Task<ActionResult> Authenticate(Credential credential)
        {
            var user = await this.UserManager.Users.FirstOrDefaultAsync(x => x.Email == credential.Email);
            if (user == null) return BadRequest();

            var userVallid = await this.UserManager.CheckPasswordAsync(user, credential.Password);

            if (!userVallid) return BadRequest();

            var claims = await this.UserManager.GetClaimsAsync(user);

            var expiresAt = DateTime.UtcNow.AddMinutes(30);


            return Ok(new
            {
                access_token = GenerateJWTToken(claims, expiresAt),
                expires_at = expiresAt
            });
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
        private string GenerateJWTToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            //HmacSha256Signature use 32-character key
            var key = Encoding.ASCII.GetBytes(this.configuration.GetValue<string>("SecretKey") ?? string.Empty);

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

    public class Credential
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
