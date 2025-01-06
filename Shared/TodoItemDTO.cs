using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
    public class TodoItemDTO
    {

        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public DateTime DueDate { get; set; }
    }
}
