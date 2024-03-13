using System.Net;
using System.Net.Http.Json;
using TodoApi.Models;


namespace TodoApi.IntegrationTests
{
    public class TodoItemsControllerIntegrationTest
    {

        [Fact]
        public async Task GetAllItemsRequest()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/todoitems");

            // Assert
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<List<TodoItemDTO>>();
            Assert.NotNull(items);
            Assert.Equal(3, items.Count);
            Assert.Equal("Item1", items[0].Name);

            // Clean 
            application.Dispose();
        }

        [Fact]
        public async Task GetItemByIdRequest()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();
            long id = 1;

            // Act
            var response = await client.GetAsync($"/api/todoitems/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var item = await response.Content.ReadFromJsonAsync<TodoItemDTO>();
            Assert.NotNull(item);
            Assert.Equal("Item1", item.Name);

            // Clean up
            application.Dispose();
        }

        [Fact]
        public async Task CreateItemRequest()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();
            var newItem = new TodoItemDTO { Name = "ItemAdded", IsComplete = false };

            // Act
            var response = await client.PostAsJsonAsync("/api/todoitems", newItem);

            // Assert
            response.EnsureSuccessStatusCode();
            var itemResponse = await response.Content.ReadFromJsonAsync<TodoItemDTO>();
            Assert.NotNull(itemResponse);
            Assert.Equal(newItem.Name, itemResponse.Name);

            // Clean up
            application.Dispose();
        }

        [Fact]
        public async Task UpdateItemRequest()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();
            long id = 1;
            var updatedItem = new TodoItemDTO { Id = id, Name = "Item1Updated", IsComplete = true };

            // Act
            var response = await client.PutAsJsonAsync($"/api/todoitems/{id}", updatedItem);

            // Assert
            response.EnsureSuccessStatusCode();
            var itemResponse = await response.Content.ReadFromJsonAsync<TodoItemDTO>();
            Assert.NotNull(itemResponse);
            Assert.Equal(updatedItem.Name, itemResponse.Name);

            // Clean up
            application.Dispose();
        }

        [Fact]
        public async Task DeleteItemRequest()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();
            long id = 1;

            // Act
            var response = await client.DeleteAsync($"/api/todoitems/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteItemRequestFailure()
        {
            // Arrange
            var application = new TodoWebApplicationFactory();
            var client = application.CreateClient();
            long id = 100;

            // Act
            var response = await client.DeleteAsync($"/api/todoitems/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}