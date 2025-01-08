using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;
using WebAppRazor.Services;

namespace WebAppRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<TodoItemDTO> todoItems { get; set; }
        public ITodoService TodoService { get; }

        public IndexModel(ILogger<IndexModel> logger, ITodoService todoService)
        {
            _logger = logger;
            TodoService = todoService;
            this.todoItems = new();
        }
        

        public async Task OnGetAsync()
        {
            todoItems = await this.TodoService.GetTodosAsync();
        }
    }
}
