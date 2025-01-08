using System.Text.Json;
using WebAppRazor.DTOs;

namespace WebAppRazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Token?> GenerateTokenAsync(string userName, string password)
        {
            var httpClient = this.httpClientFactory.CreateClient("AuthApi");
            var responseMessage = await httpClient.PostAsJsonAsync<Credential>("Auth", new Credential
            {
                Email = userName,
                Password = password
            });

            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            if (responseBody == null)
            {
                return new Token();
            }

            return JsonSerializer.Deserialize<Token>(responseBody);
        }

    }
}
