using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly IHttpClientFactory HttpClientFactory;

        [BindProperty]
        public List<TodoItemDTO> todoItems { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            HttpClientFactory = httpClientFactory;
            this.todoItems = new();
        }
        

        public async Task OnGetAsync()
        {
            var httpClient = this.HttpClientFactory.CreateClient("WebApi");
            var data = await httpClient.GetStringAsync("TodoItems");
            Console.WriteLine(data);

            var xx = JsonSerializer.Deserialize<List<TodoItemDTO>>(data);

            todoItems = await httpClient.GetFromJsonAsync<List<TodoItemDTO>>("TodoItems") ?? new List<TodoItemDTO>();
        }
    }
}
