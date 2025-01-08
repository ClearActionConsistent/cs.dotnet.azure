using System.Net.Http.Headers;
using System.Text.Json;
using Shared;

namespace WebAppRazor.Services
{
    public class TodoService : ITodoService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public readonly IAuthService AuthService;

        public TodoService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            this.httpClientFactory = httpClientFactory;
            AuthService = authService;
        }

        public async Task<List<TodoItemDTO>> GetTodosAsync()
        {
            var token = await this.AuthService.GenerateTokenAsync("user15@test.com","123456789aA!");

            var httpClient = this.httpClientFactory.CreateClient("TodoApi");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.access_token ?? string.Empty);
            return await httpClient.GetFromJsonAsync<List<TodoItemDTO>>("TodoItems") ?? new List<TodoItemDTO>();
        }
    }
}
