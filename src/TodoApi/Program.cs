using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add db context for mysql database
builder.Services.AddDbContextPool<TodoContext>(opt =>
    opt.UseMySql(builder.Configuration.GetConnectionString("TodoContext"), new MySqlServerVersion(new Version()),
           mySqlOptions => mySqlOptions.EnableRetryOnFailure()));

// Add environment variable for database connection string
builder.Configuration.AddEnvironmentVariables();

/*builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register TodoService
builder.Services.AddScoped<ITodoItemService, TodoItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
