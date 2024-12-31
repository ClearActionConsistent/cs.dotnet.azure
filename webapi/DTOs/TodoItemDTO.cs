using System.ComponentModel.DataAnnotations;
using webapi.CustomAttibutes.ValidationAttributes;

namespace webapi.DTOs
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        [FinishBeforYearAtribute(2025)]
        public DateTime DueDate { get; set; }
    }
}
