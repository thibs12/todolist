using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var host = Configuration["DBHOST"] ?? "localhost";
            var port = Configuration["DBPORT"] ?? "3306";
            var password = Configuration["DBPASSWORD"] ?? "password";

            services.AddDbContextPool<TodoContext>(options => 
            options.UseMySql($"server={host};port={port};user=thibs;password={password};database=todolist;", 
                new MySqlServerVersion(new Version())));
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<ITodoItemService, TodoItemService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:"{controller=TodoItems}/{action=Index}/{id?}");
            });
        }
    }
}
