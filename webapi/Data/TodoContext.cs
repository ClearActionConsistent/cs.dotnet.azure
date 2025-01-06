using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) => {
                var newTodo = new TodoItem
                {
                    Id = 1,
                    DueDate = DateTime.Now,
                    IsComplete = false,
                    Name = "Todo 1",
                    Secret = "secret"
                };

                context.Set<TodoItem>().Add(newTodo);
                context.SaveChanges();
            });

            optionsBuilder.UseSeeding(async (context, _) => {
                var newTodo = new TodoItem
                {
                    Id = 1,
                    DueDate = DateTime.Now,
                    IsComplete = false,
                    Name = "Todo 1",
                    Secret = "secret"
                };

                await context.Set<TodoItem>().AddAsync(newTodo);
                await context.SaveChangesAsync();
            });

        }
    }
}
