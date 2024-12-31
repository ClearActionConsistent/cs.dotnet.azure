using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
