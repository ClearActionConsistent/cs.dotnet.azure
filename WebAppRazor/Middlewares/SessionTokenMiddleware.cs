using Microsoft.Extensions.DependencyInjection;
using WebAppRazor.Services;

namespace WebAppRazor.Middlewares
{
    public class SessionTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var token = context.Session.GetString("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                authService.Token = token;
            }
            await _next(context);
        }
    }
}
