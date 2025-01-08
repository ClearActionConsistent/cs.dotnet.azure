using System.Text.Json;
using Shared;

namespace WebAppRazor.Services
{
    public class TodoService : ITodoService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TodoService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<List<TodoItemDTO>> GetTodosAsync()
        {
            var httpClient = this.httpClientFactory.CreateClient("TodoApi");
            return await httpClient.GetFromJsonAsync<List<TodoItemDTO>>("TodoItems") ?? new List<TodoItemDTO>();
        }
    }
}
