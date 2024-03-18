using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoApi.Models;

namespace TodoApi.IntegrationTests
{
    internal class TodoWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TodoContext>));
                services.AddDbContext<TodoContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TodoContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.TodoItems.Add(new TodoItem { Name = "Item1", IsComplete = false });
                context.TodoItems.Add(new TodoItem { Name = "Item2", IsComplete = false });
                context.TodoItems.Add(new TodoItem { Name = "Item3", IsComplete = false });
                context.SaveChanges();

            });
        }
    }
}
