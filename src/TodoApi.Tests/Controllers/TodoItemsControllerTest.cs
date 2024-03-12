using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Tests.Controllers
{
    public class TodoItemsControllerTest
    {
        private Mock<ITodoItemService> _todoItemService;

        public TodoItemsControllerTest()
        {
            _todoItemService = new Mock<ITodoItemService>();
        }

        [Fact]
        public async void GetAll_TodoItem_Success()
        {
            // Arrange
            var returnItems = new List<TodoItemDTO>
            {
                new TodoItemDTO { Name = "Item1", IsComplete = false },
                new TodoItemDTO { Name = "Item2", IsComplete = false },
                new TodoItemDTO { Name = "Item3", IsComplete = false }
            };
            _todoItemService.Setup(service => service.GetTodoItems()).ReturnsAsync(returnItems);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.GetTodoItems();

            // Assert
            var items = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
            var list = Assert.IsType<List<TodoItemDTO>>(items.Value);

            Assert.NotNull(list);
            Assert.IsType<List<TodoItemDTO>>(list);
            Assert.Equal(returnItems.Count(), list.Count());
        }

        [Fact]
        public async void GetById_TodoItem_Success()
        {
            long id = 1;
            var returnItem = new TodoItemDTO
            {
                Id = id,
                Name = "Item1",
                IsComplete = false
            };
            // Arrange
            _todoItemService.Setup(service => service.GetTodoItem(id)).ReturnsAsync(returnItem);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.GetTodoItem(id);

            // Assert
            var createdActionResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var itemFromDb = Assert.IsType<TodoItemDTO>(createdActionResult.Value);
            Assert.NotNull(itemFromDb); 
            Assert.Equal(returnItem.Name, itemFromDb.Name);
            Assert.Equal(returnItem.IsComplete, itemFromDb.IsComplete);
            Assert.Equal(returnItem.Id, itemFromDb.Id);
        }

        [Fact]
        public async void GetById_TodoItem_Failure()
        {
            long id = 100;
            TodoItemDTO? returnItem = null;
            // Arrange
            _todoItemService.Setup(service => service.GetTodoItem(id)).ReturnsAsync(returnItem);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.GetTodoItem(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async void Add_TodoItem_Success()
        {
            // Arrange
            var itemToAdd = new TodoItemDTO
            {
                Name = "ItemAdded",
                IsComplete = false
            };
            var returnItem = new TodoItemDTO
            {
                Id = 4,
                Name = itemToAdd.Name,
                IsComplete = itemToAdd.IsComplete
            };
            _todoItemService.Setup(service => service.CreateTodoItem(itemToAdd)).ReturnsAsync(returnItem);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.PostTodoItem(itemToAdd);

            // Assert
            var createdActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var itemAdded = Assert.IsType<TodoItemDTO>(createdActionResult.Value);

            Assert.NotNull(itemAdded);
            Assert.Equal(returnItem.Name, itemAdded.Name);
            Assert.Equal(returnItem.IsComplete, itemAdded.IsComplete);
            Assert.Equal(returnItem.Id, itemAdded.Id);
        }
        
        [Fact]
        public async void Edit_TodoItem_Success()
        {
            // Arrange
            var itemToUpdate = new TodoItemDTO
            {
                Id = 1,
                Name = "ItemUpdated",
                IsComplete = true
            };
            var returnItem = new TodoItemDTO
            {
                Id = itemToUpdate.Id,
                Name = itemToUpdate.Name,
                IsComplete = itemToUpdate.IsComplete
            };
            _todoItemService.Setup(service => service.UpdateTodoItem(itemToUpdate.Id, itemToUpdate)).ReturnsAsync(returnItem);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.PutTodoItem(itemToUpdate.Id, itemToUpdate);

            // Assert
            var createdActionResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var itemUpdated = Assert.IsType<TodoItemDTO>(createdActionResult.Value);
            Assert.NotNull(itemUpdated);
            Assert.Equal(returnItem.Name, itemUpdated.Name);
            Assert.Equal(returnItem.IsComplete, itemUpdated.IsComplete);
            Assert.Equal(returnItem.Id, itemUpdated.Id);
        }

        [Fact]
        public async void Edit_TodoItem_BadRequest()
        {
            // Arrange
            var itemToUpdate = new TodoItemDTO
            {
                Id = 1,
                Name = "ItemUpdated",
                IsComplete = true
            };
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.PutTodoItem(100, itemToUpdate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Edit_TodoItem_NotFound()
        {
            // Arrange
            var itemToUpdate = new TodoItemDTO
            {
                Id = 100,
                Name = "ItemUpdated",
                IsComplete = true
            };
            TodoItemDTO? returnItem = null;
            _todoItemService.Setup(service => service.UpdateTodoItem(itemToUpdate.Id, itemToUpdate)).ReturnsAsync(returnItem);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.PutTodoItem(100, itemToUpdate);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async void Delete_TodoItem_Success()
        {
            // Arrange
            var itemToDelete = new TodoItemDTO
            {
                Id = 1,
                Name = "Item1",
                IsComplete = false
            };
            _todoItemService.Setup(service => service.DeleteTodoItem(itemToDelete.Id)).ReturnsAsync(true);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.DeleteTodoItem(itemToDelete.Id);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async void Delete_TodoItem_Failure()
        {
            // Arrange
            var itemToDelete = new TodoItemDTO
            {
                Id = 100,
                Name = "Item1",
                IsComplete = false
            };
            _todoItemService.Setup(service => service.DeleteTodoItem(itemToDelete.Id)).ReturnsAsync(false);
            var controller = new TodoItemsController(_todoItemService.Object);

            // Act
            var result = await controller.DeleteTodoItem(itemToDelete.Id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(notFoundResult);
        }
    }
}