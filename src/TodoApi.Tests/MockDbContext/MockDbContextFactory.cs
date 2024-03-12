using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Tests.MockDbContext
{
    public class MockDbContextFactory
    {
        public TodoContext DbContextFactory()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoListTest").Options;
            var context = new TodoContext(options);
            
            context.Database.EnsureCreated();
            context.TodoItems.Add(new TodoItem { Name = "Item1", IsComplete = false });
            context.TodoItems.Add(new TodoItem { Name = "Item2", IsComplete = false });
            context.TodoItems.Add(new TodoItem { Name = "Item3", IsComplete = false });
            
            context.SaveChanges();

            return context;
        }
    }
}

