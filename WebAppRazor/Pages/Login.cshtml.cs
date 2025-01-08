using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppRazor.DTOs;
using WebAppRazor.Services;

namespace WebAppRazor.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Token? token { get; set; }
        private readonly IAuthService authService;

        public LoginModel(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task OnGet()
        {
            Token? token;
            var strToken = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrEmpty(strToken))
            {
                token = await authService.GenerateTokenAsync("user15@test.com","123456789aA!");
                
                if(token != null)
                    HttpContext.Session.SetString("access_token", JsonSerializer.Serialize(token));
            }
            else
            {
                token = JsonSerializer.Deserialize<Token>(strToken);
            }

            this.token = token;
        }
    }
}
