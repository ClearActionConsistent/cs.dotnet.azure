using System.Text.Json;
using WebAppRazor.DTOs;

namespace WebAppRazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public string Token {  get; set; }

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.Token = string.Empty;
        }

        public async Task<Token?> GenerateTokenAsync(string userName, string password)
        {
            if (!string.IsNullOrEmpty(this.Token)) {
                return JsonSerializer.Deserialize<Token>(this.Token);
            }

            var httpClient = this.httpClientFactory.CreateClient("AuthApi");
            var responseMessage = await httpClient.PostAsJsonAsync<Credential>("Auth", new Credential
            {
                Email = userName,
                Password = password
            });

            responseMessage.EnsureSuccessStatusCode();

            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            if (responseBody == null)
            {
                return new Token();
            }

            return JsonSerializer.Deserialize<Token>(responseBody);
        }

    }
}
