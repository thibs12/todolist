using TodoApi.Models;
using TodoApi.Services;
using TodoApi.UnitTests.MockDbContext;

namespace TodoApi.UnitTests.Services
{
    public class TodoItemServiceTest
    {
        public MockDbContextFactory contextFactory;

        public TodoItemServiceTest()
        {
            contextFactory = new MockDbContextFactory();
        }

        [Fact]
        public async Task GetTodoItems_ReturnsAllItems()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);

            // Act
            var items = await service.GetTodoItems();

            // Assert
            Assert.NotNull(items);
            Assert.IsType<List<TodoItemDTO>>(items);
            Assert.Equal(3, items.Count());

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetbyId_ReturnsItem()
        {
            long id = 1;
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);

            // Act
            var item = await service.GetTodoItem(id);

            // Assert
            Assert.NotNull(item);
            Assert.IsType<TodoItemDTO>(item);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetbyId_Failure()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);

            // Act
            var item = await service.GetTodoItem(0);

            // Assert
            Assert.Null(item);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task CreateTodoItem_ReturnsItem()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);
            var item = new TodoItemDTO
            {
                Name = "ItemAdded",
                IsComplete = false
            };

            // Act
            var result = await service.CreateTodoItem(item);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TodoItemDTO>(result);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task UpdateTodoItem_ReturnsItem()
        {
            long id = 1;
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);
            var item = new TodoItemDTO
            {
                Name = "ItemUpdated",
                IsComplete = true
            };

            // Act
            var result = await service.UpdateTodoItem(id, item);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TodoItemDTO>(result);

            context.Database.EnsureDeleted();
        }

        // test update failure
        [Fact]
        public async Task UpdateTodoItem_Failure()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);
            var todoItemDTO = new TodoItemDTO();

            // Act
            var result = await service.UpdateTodoItem(0, todoItemDTO);

            // Assert
            Assert.Null(result);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsTrue()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);

            // Act
            var result = await service.DeleteTodoItem(1);

            // Assert
            Assert.True(result);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task DeleteTodoItem_Failure()
        {
            // Arrange
            var context = contextFactory.DbContextFactory();
            var service = new TodoItemService(context);

            // Act
            var result = await service.DeleteTodoItem(0);

            // Assert
            Assert.False(result);

            context.Database.EnsureDeleted();
        }
    }
}
