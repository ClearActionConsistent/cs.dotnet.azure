using Shared;

namespace WebAppRazor.Services
{
    public interface ITodoService
    {
        Task<List<TodoItemDTO>> GetTodosAsync();
    }
}