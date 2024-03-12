using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Models;
using Xunit.Abstractions;

namespace TodoApi.Tests
{
    public class TodoItemsControllerTest
    {
        // Output
        private readonly ITestOutputHelper output;

        // Controller
        private readonly TodoContext _context;
        DbContextOptions<TodoContext> options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoListTest").Options;
        TodoItemsController _controller;

        public TodoItemsControllerTest(ITestOutputHelper output)
        {
            this.output = output;
            _context = new TodoContext(options);
            _controller = new TodoItemsController(_context);
        }

        // On ajoute des données dans la base de données
        private void EnsureSeed()
        {
            _context.TodoItems.Add(new TodoItem { Name = "Item1", IsComplete = false });
            _context.TodoItems.Add(new TodoItem { Name = "Item2", IsComplete = false });
            _context.TodoItems.Add(new TodoItem { Name = "Item3", IsComplete = false });
            _context.SaveChanges();
        }

        // On vide la base de données
        private void EnsureDeleted()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async void GetAll_TodoItem_Success()
        {
            // Arrange
            EnsureSeed();

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var items = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);

            Assert.NotNull(items);
            Assert.IsType<List<TodoItemDTO>>(items.Value);
            Assert.Equal(3, items.Value?.Count());

            EnsureDeleted();
        }

        [Fact]
        public async void GetById_TodoItem_Success()
        {
            // Arrange
            var todoItem = new TodoItem
            {
                Name = "ItemAdded",
                IsComplete = false
            };
            _context.TodoItems.Add(todoItem);

            // get id of the added item
            _context.SaveChanges();

            // Act
            var result = await _controller.GetTodoItem(todoItem.Id);
            var resultError = await _controller.GetTodoItem(100);

            // Assert
            var createdActionResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var itemFromDb = Assert.IsType<TodoItemDTO>(createdActionResult.Value);
            Assert.NotNull(itemFromDb);
            // check values
            Assert.Equal(todoItem.Name, itemFromDb.Name);
            Assert.Equal(todoItem.IsComplete, itemFromDb.IsComplete);
            Assert.Equal(todoItem.Id, itemFromDb.Id);

            Assert.IsType<NotFoundResult>(resultError.Result);

            EnsureDeleted();
        }

        [Fact]
        public async void Add_TodoItem_Success()
        {
            // Arrange
            var todoItem = new TodoItemDTO
            {
                Name = "ItemAdded",
                IsComplete = false
            };

            // Act
            var result = await _controller.PostTodoItem(todoItem);

            // Assert
            var createdItem = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<TodoItemDTO>(createdItem.Value);

            // value checks
            Assert.Equal(todoItem.Name, model.Name);
            Assert.Equal(todoItem.IsComplete, model.IsComplete);
            Assert.True(model.Id > 0);

            EnsureDeleted();

        }

        [Fact]
        public async void Edit_TodoItem_Success()
        {
            // Arrange
            var todoItem = new TodoItem
            {
                Name = "ItemAdded",
                IsComplete = false
            };
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            // Act
            // get item from db
            var todoItemToUpdate = await _context.TodoItems.FindAsync(todoItem.Id);
            TodoItemDTO todoItemToUpdateDTO = new TodoItemDTO
            {
                Id = todoItemToUpdate.Id,
                Name = todoItemToUpdate.Name,
                IsComplete = todoItemToUpdate.IsComplete
            };

            // update the item
            todoItemToUpdateDTO.Name = "ItemUpdated";
            todoItemToUpdateDTO.IsComplete = true;

            var result = await _controller.PutTodoItem(todoItem.Id, todoItemToUpdateDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
            // check if the item is updated
            var itemFromDb = await _context.TodoItems.FindAsync(todoItem.Id);
            Assert.NotNull(itemFromDb);

            // value checks
            Assert.Equal(todoItemToUpdateDTO.Name, itemFromDb.Name);
            Assert.Equal(todoItemToUpdateDTO.IsComplete, itemFromDb.IsComplete);
            Assert.Equal(todoItemToUpdateDTO.Id, itemFromDb.Id);

            EnsureDeleted();
        }

        [Fact]
        public async void Delete_TodoItem_Success()
        {
            // Arrange
            var todoItem = new TodoItem
            {
                Name = "ItemAdded",
                IsComplete = false
            };
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
            // Act
            var result = await _controller.DeleteTodoItem(todoItem.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            // check if the item is deleted
            var itemFromDb = await _context.TodoItems.FindAsync(todoItem.Id);
            Assert.Null(itemFromDb);
        }
    }
}