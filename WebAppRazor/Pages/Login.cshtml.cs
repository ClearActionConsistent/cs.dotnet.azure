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
            this.token = await this.authService.GenerateTokenAsync("user15@test.com", "123456789aA!");
        }
    }
}
